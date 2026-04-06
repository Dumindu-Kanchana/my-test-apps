import ballerina/http;
import ballerina/log;

service / on new http:Listener(8090) {

    resource function post github/event(http:Request req) returns http:Response|error {
        string|http:HeaderNotFoundError eventHeader = req.getHeader("X-GitHub-Event");
        string event = eventHeader is string ? eventHeader : "unknown";

        json|error payload = req.getJsonPayload();
        if payload is json {
            log:printInfo("GitHub webhook received", githubEvent = event, payload = payload.toString());
        } else {
            string body = check req.getTextPayload();
            log:printInfo("GitHub webhook received", githubEvent = event, body = body);
        }

        http:Response res = new;
        res.statusCode = 200;
        res.setPayload("Webhook received");
        return res;
    }
}
