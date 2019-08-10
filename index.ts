import "reflect-metadata";
import "./utils/express.extension";
import "./utils/array.extension";
import { AppModule } from "./app/app.module";
import express = require("express");

const server = express();
server
    .useGeexGraphql({
        entryModuleOption: { imports: [AppModule] },
        useAccounts: true,
        connectionString: 'mongodb://XxusernamexX:XxpasswordxX@localhost:27017/test?authSource=admin'
    })
    .then(x => {
        console.log(`🚀 Server ready at http://localhost:4000/graphql`);
        x.listen(4000)
    })
