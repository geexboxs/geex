import "reflect-metadata";
import "./server/utils/express.extension";
import "./utils/array.extension";
import { AppModule } from "./app/app.module";
import { connect } from "mongoose";
import express = require("express");

let hehe = [1, 2, 3];
// @ts-ignore
console.log(hehe.add(4));


connect('mongodb://XxusernamexX:XxpasswordxX@localhost:27017/test?authSource=admin');

const server = express();
server
    .useGeexGraphql({ imports: [AppModule], useAccounts: true })
    .then(x => {
        console.log(`🚀 Server ready at http://localhost:4000/graphql`);
        x.listen(4000)
    })
