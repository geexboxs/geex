import { ModelType, InstanceType } from 'typegoose';
import { Address, AddressModel } from './domain/models/address.model';
import { User, UserModel } from './domain/models/user.model';
import { connect } from 'mongoose';
import { AppModule } from './app/app.module';
import express from 'express';
import graphqlHTTP from 'express-graphql';
import { Context } from './app/auth/context.interface';
import { ApolloServer } from "apollo-server-express";
import { applyMiddleware } from 'graphql-middleware'
import bodyParser from 'body-parser';
import { globalLoggingMiddleware } from './shared/logging/globalLogging.middleware';

connect('mongodb://root:123456@localhost:27017/test?authSource=admin');

const app = express()
app.use(bodyParser())
app.use(globalLoggingMiddleware);

// UserModel is a regular Mongoose Model with correct types
(async () => {
    const server = new ApolloServer({
        schema: AppModule.schema,
        context: AppModule.context,
        resolvers: AppModule.resolvers,
    });
    server.applyMiddleware({ app })
    // Start the server
    app.listen({ port: 4000 }, () =>
        console.log(`🚀 Server ready at http://localhost:4000${server.graphqlPath}`)
    );
})()


