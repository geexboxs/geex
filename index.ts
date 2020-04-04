import "./utils/array.extension";
import "./utils/date.extension";
import "reflect-metadata";
import express = require("express");
import { appConfig } from "./configs/app-config";
import { AppModule } from "./app/app.module";
import { Passport } from "passport";
import { ApolloServer } from "apollo-server-express";
import passport = require("passport");
import { ValidationPipe } from "@nestjs/common";
import { NestFactory } from "@nestjs/core";
async function main() {
    const app = await NestFactory.create(AppModule);
    app.useGlobalPipes(new ValidationPipe());

    await app.listen(4000);
    console.log(`Application is running on: ${await app.getUrl()}`);
}

main();
