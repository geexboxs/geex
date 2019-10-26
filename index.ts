import "./utils/array.extension";
import "./utils/date.extension";
import "reflect-metadata";
import express = require("express");
import { environment } from "./environments/environment";
import { AppModule } from "./app/app.module";
import { Passport } from "passport";
import { ApolloServer } from "apollo-server-express";
import passport = require("passport");
async function main() {
    const entryModule = await AppModule;
    const app = express();
    app.use(express.json());
    app.use(entryModule.injector.get(Passport));
    app.all("/*", (req, res, next) => {
        passport.authenticate("jwt", { session: false }, (err, user, info) => {
            if (user) {
                req.user = user;
            }
            next();
        })(req, res, next);
    });
    entryModule.injector.get(ApolloServer).applyMiddleware({ app });
    app.listen(environment.port, environment.hostname);
    console.log(`Server ready at http://${environment.hostname}:${environment.port}/graphql`);
}

main();
