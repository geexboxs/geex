import "reflect-metadata";
import "./shared/utils/array.extension";
import express = require("express");
import { SharedModule } from "./shared/shared.module";
import { ApolloServer } from "apollo-server-express";
import { GeexLogger } from "./shared/utils/logger";
import { RequestIdentityExtension } from "./extensions/request-identity.gql-extension";

const entryModule = SharedModule;
const app = express();
const apollo = new ApolloServer({
    context: entryModule.context,
    schema: entryModule.schema,
    uploads: {
        maxFileSize: 100000000,
        maxFiles: 10,
    },
    // formatError: error => {
    //     entryModule.injector.get(GeexLogger).error(error);
    //     return error;
    // },
    extensions: [RequestIdentityExtension]
});

app.use(express.json());
// 将 graphql server 挂载到 express
apollo.applyMiddleware({ app: app });
// 注入 graphql server 到 entryModule
// entryModule.selfProviders.push({
//     provide: ApolloServer,
//     useValue: ApolloServer
// });
// entryModule.selfProviders.push({
//     provide: ExpressToken,
//     useValue: app
// });
app.listen(4000)

console.log(`Server ready at http://localhost:4000/graphql`);
