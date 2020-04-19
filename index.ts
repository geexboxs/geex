import "./patches/custom-patches/array.patch";
import "./patches/custom-patches/date.patch";
import "./patches/custom-patches/typegoose.patch";
import "./patches/custom-patches/nestjs.patch";
import "reflect-metadata";
import express = require("express");
import { appConfig } from "./configs/app-config";
import { AppModule } from "./app/app.module";
import { Passport } from "passport";
import { ApolloServer } from "apollo-server-express";
import passport = require("passport");
import { ValidationPipe } from "@nestjs/common";
import { NestFactory, INestApplication } from "@nestjs/core";
import { ServiceLocator } from "./shared/utils/service-locator";
async function main() {
    const app = await NestFactory.create(AppModule);
    app.useGlobalPipes(new ValidationPipe());
    ServiceLocator.config(app);
    await app.listen(4000);
    console.log(`Application is running on: ${await app.getUrl()}`);
}

main();
