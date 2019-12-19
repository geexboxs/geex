import { GraphQLModule } from "@graphql-modules/core";
import { buildSchemaSync } from "type-graphql";
import { JaegerTraceExtension } from "../shared/extensions/jaeger-trace.gql-extension";
import { ComplexityExtension } from "../shared/extensions/complexity.gql-extension";
import { AuditLogModule } from "./audit-log/audit-log.module";
import express = require("express");
import { ApolloServer } from "apollo-server-express";
import passport = require("passport");
import { mongoose } from "@typegoose/typegoose";
import { appConfig } from "../configs/app-config";
import { Passport } from "passport";
import { buildContext } from "graphql-passport";
import { GeexServerConfigToken as AppConfigToken } from "../shared/tokens";
import Redis = require("ioredis");
import { AccountModule } from "./account/account.module";
import { SessionModule } from "./session/session.module";
// tslint:disable-next-line: no-var-requires
async function preInitialize() {
    return;
}
async function postInitialize(self: GraphQLModule) {
    await mongoose.connect(appConfig.connections.mongo, { useNewUrlParser: true, useUnifiedTopology: true });
}

export const AppModule: Promise<GraphQLModule> = (async () => {

    await preInitialize();
    const self: GraphQLModule = new GraphQLModule({
        providers: [
            {
                provide: ApolloServer,
                useFactory: (injector) => {
                    const apollo = new ApolloServer({
                        context: async (session) => await self.context(buildContext(session)),
                        schema: self.schema,
                        uploads: {
                            maxFileSize: 100000000,
                            maxFiles: 10,
                        },
                        tracing: true,
                        extensions: [
                            () => self.injector.get(JaegerTraceExtension),
                            () => self.injector.get(ComplexityExtension),
                        ],
                    });
                    return apollo;
                },
            },
            {
                provide: AppConfigToken,
                useValue: appConfig,
            },
            JaegerTraceExtension,
            ComplexityExtension,
        ],
        imports: [AuditLogModule, await AccountModule, await SessionModule],
    });
    await postInitialize(self);
    return self;
})();
