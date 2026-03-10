# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Overview

This is a mono-repo containing sample "Hello World" integration apps built with two different WSO2 technologies:

- **`hello-app-ballerina/`** — A Ballerina HTTP service (v2201.12.7)
- **`hello-app-mi/`** — A WSO2 Micro Integrator (MI) integration project (runtime v4.5.0)

## hello-app-ballerina (Ballerina)

### Commands

```bash
# Run the service (listens on port 9090)
cd hello-app-ballerina && bal run

# Run tests
cd hello-app-ballerina && bal test

# Build
cd hello-app-ballerina && bal build
```

### Architecture

- `service.bal` — Single-file service using `http:InterceptableService` on port 9090. Implements request/response interceptors for logging elapsed time per request. Exposes `GET /hello/{name}`.
- `tests/service_test.bal` — Test file (currently empty).

## hello-app-mi (WSO2 Micro Integrator)

### Commands

```bash
# Build the Carbon Application (.car file)
cd hello-app-mi && ./mvnw clean install

# Build with Docker image
cd hello-app-mi && ./mvnw clean install -P docker

# Run tests (uses synapse-unit-test-maven-plugin against a local MI server on port 9008)
cd hello-app-mi && ./mvnw test

# Skip tests
cd hello-app-mi && ./mvnw clean install -DskipTests
```

### Architecture

- **Artifacts** live under `src/main/wso2mi/artifacts/` — APIs are defined as Synapse XML files (e.g., `HelloWorldAPI.xml`).
- **API definition**: `HelloWorldAPI.xml` exposes `GET /hello/world`, returning `{"message": "Hello World!"}`.
- **OpenAPI spec**: `src/main/wso2mi/resources/api-definitions/HelloWorldAPI.yaml`.
- **Deployment config**: `deployment/deployment.toml` — MI server configuration (keystore, transport, etc.).
- **Docker**: `deployment/docker/Dockerfile` copies the built `.car` file and keystores into a `wso2/wso2mi:4.5.0` base image.
- **Build output**: Maven packages artifacts into a `.car` (Carbon Application) file deployed to MI.
- The `pom.xml` uses `vscode-car-plugin` (5.4.13) to package the CAR and `synapse-unit-test-maven-plugin` for testing. The `docker` Maven profile additionally runs `mi-container-config-mapper` to transform config files.
- Dependency: `mi-connector-http` (0.1.14) for HTTP connector support.
