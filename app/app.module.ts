import { GraphQLModule } from "@graphql-modules/core";
import { buildSchemaSync } from "type-graphql";
import { JaegerTraceExtension } from "../shared/extensions/jaeger-trace.gql-extension";
import { ComplexityExtension } from "../shared/extensions/complexity.gql-extension";
import { AuditLogModule } from "./audit-log/audit-log.module";
import { UserModule } from "./user/user.module";
import express = require("express");
import { ApolloServer } from "apollo-server-express";
import passport = require("passport");
import { mongoose } from "@typegoose/typegoose";
import { environment } from "../environments/environment";

async function preInitialize() {
}
preInitialize();
export const AppModule: GraphQLModule = new GraphQLModule({
    providers: [
        {
            provide: express,
            useFactory: (injector) => {
                const app = express();
                app.use(express.json());
                app.use(passport.initialize());
                injector.get(ApolloServer).applyMiddleware({ app });
                return app;
            },
        },
        {
            provide: ApolloServer,
            useFactory: (injector) => {
                const apollo = new ApolloServer({
                    context: async (session) => await AppModule.context(session),
                    schema: AppModule.schema,
                    uploads: {
                        maxFileSize: 100000000,
                        maxFiles: 10,
                    },
                    tracing: true,
                    extensions: [
                        () => AppModule.injector.get(JaegerTraceExtension),
                        () => AppModule.injector.get(ComplexityExtension),
                    ],
                });
                return apollo;
            },
        },
        JaegerTraceExtension,
        ComplexityExtension,
    ],
    imports: [AuditLogModule, UserModule],
});
async function postInitialize() {
    await mongoose.connect(environment.connectionString, { useNewUrlParser: true, useUnifiedTopology: true });
}
postInitialize();
