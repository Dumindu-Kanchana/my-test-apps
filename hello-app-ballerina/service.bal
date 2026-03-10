import ballerina/http;
import ballerina/log;
import ballerina/time;

configurable string appName = "HelloApp";
configurable string environment = ?;

// 1. Request Interceptor: Captures the start time
service class RequestLogInterceptor {
    *http:RequestInterceptor;

    isolated resource function 'default [string... path](http:RequestContext ctx, http:Request req)
            returns http:NextService|error? {

        // Store the start time in the context to access it in the response phase
        ctx.set("start_time", time:utcNow());
        // Convert the array back into a readable string
        string fullPath = "/" + string:'join("/", ...path);
        // Store the full path in the context for logging in the response interceptor
        ctx.set("request_path", fullPath);
        return ctx.next();
    }
}

// 2. Response Interceptor: Calculates duration and logs the result
service class ResponseLogInterceptor {
    *http:ResponseInterceptor;

    isolated remote function interceptResponse(http:RequestContext ctx, http:Response res)
            returns http:NextService|error? {

        // Retrieve the start time
        var startTime = ctx.get("start_time");

        if startTime is time:Utc {
            time:Utc endTime = time:utcNow();
            time:Seconds diff = time:utcDiffSeconds(endTime, startTime);

            // Retrieve the request path for logging
            var path = ctx.get("request_path");
            if path is string {
                log:printInfo(string `Executed ${path}`,
                        elapsedTime = diff,
                        status = res.statusCode);
            }
        }

        return ctx.next();
    }
}

// Define your service as an http:InterceptableService
service http:InterceptableService / on new http:Listener(9090) {

    // This function tells Ballerina which interceptors to use for this service
    public function createInterceptors() returns [RequestLogInterceptor, ResponseLogInterceptor] {
        return [new RequestLogInterceptor(), new ResponseLogInterceptor()];
    }

    resource function get hello/[string name]() returns string {
        return string `Hello, ${name}!`;
    }

    resource function get hello/[string name]/[string greeting]() returns string {
        return string `[${appName}/${environment}] ${greeting}, ${name}!`;
    }
}
