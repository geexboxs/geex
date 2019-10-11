import "reflect-metadata";
import './graphql-patch';
import "./shared/server/express.extension";
import "./shared/utils/array.extension";
import express = require("express");

const server = express();
server
    .useGeexGraphql({
        imports: [],
        providers: [],
        connectionString: 'mongodb://XxusernamexX:XxpasswordxX@localhost:27017/test?authSource=admin',
    })
    .then(x => {
        console.log(`🚀 Server ready at http://localhost:4000/graphql`);
        x.listen(4000)
    })
