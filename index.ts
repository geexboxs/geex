import "./utils/array.extension";
import "reflect-metadata";
import { ApolloServer } from "apollo-server-express";
import express = require("express");
import { environment } from "./environments/environment";
import { JaegerTraceExtension } from "./shared/extensions/jaeger-trace.gql-extension";
import { AppModule } from "./app/app.module";
import { ComplexityExtension } from "./shared/extensions/complexity.gql-extension";
import { mongoose } from "@typegoose/typegoose";
(async () => {
    await mongoose.connect(environment.connectionString, { useNewUrlParser: true, useUnifiedTopology: true});
})();

const entryModule = AppModule;
const app = express();
const apollo = new ApolloServer({
    context: async (session) => await entryModule.context(session),
    schema: entryModule.schema,
    uploads: {
        maxFileSize: 100000000,
        maxFiles: 10,
    },
    plugins: [],
    tracing: true,
    extensions: [
        () => entryModule.injector.get(JaegerTraceExtension),
        () => entryModule.injector.get(ComplexityExtension),
    ],
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
app.listen(environment.port, environment.hostname);
console.log(`Server ready at http://${environment.hostname}:${environment.port}/graphql`);
