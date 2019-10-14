import { ApolloServer } from "apollo-server-express";
import express = require("express");
import "reflect-metadata";
import { environment } from "./environments/environment";
import { GlobalLoggingExtension } from "./shared/extensions/global-logging.gql-extension";
import { RequestIdentityExtension } from "./shared/extensions/request-identity.gql-extension";
import { SharedModule } from "./shared/shared.module";
import { GeexContext } from "./shared/utils/abstractions";
import "./shared/utils/array.extension";
import { GeexLogger } from "./shared/utils/logger";

const entryModule = SharedModule;
const app = express();
const apollo = new ApolloServer({
    context: async (session) => await entryModule.context(session),
    schema: entryModule.schema,
    uploads: {
        maxFileSize: 100000000,
        maxFiles: 10,
    },
    extensions: [() => entryModule.injector.get(RequestIdentityExtension), () => entryModule.injector.get(GlobalLoggingExtension)],
});

app.use(express.json());
// 将 graphql server 挂载到 express
apollo.applyMiddleware({ app });
// 注入 graphql server 到 entryModule
// entryModule.selfProviders.push({
//     provide: ApolloServer,
//     useValue: ApolloServer
// });
// entryModule.selfProviders.push({
//     provide: ExpressToken,
//     useValue: app
// });
app.listen(4000);

console.log(`Server ready at http://localhost:4000/graphql`);
