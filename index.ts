import "./utils/array.extension";
import "reflect-metadata";
import { ApolloServer } from "apollo-server-express";
import express = require("express");
import { environment } from "./environments/environment";
import { JaegerTraceExtension } from "./shared/extensions/jaeger-trace.gql-extension";
import { AppModule } from "./app/app.module";
import { ComplexityExtension } from "./shared/extensions/complexity.gql-extension";
import { mongoose } from "@typegoose/typegoose";
import passport = require("passport");
async function main() {
    const app = AppModule.injector.get(express);
    // 将 graphql server 挂载到 express
    // 注入 graphql server 到 AppModule
    // AppModule.selfProviders.push({
    //     provide: ApolloServer,
    //     useValue: ApolloServer
    // });
    // AppModule.selfProviders.push({
    //     provide: ExpressToken,
    //     useValue: app
    // });
    app.listen(environment.port, environment.hostname);
    console.log(`Server ready at http://${environment.hostname}:${environment.port}/graphql`);
}

main();
