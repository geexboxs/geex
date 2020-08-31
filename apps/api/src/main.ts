import "./patches/custom-patches/array.patch";
import "./patches/custom-patches/date.patch";
import "./patches/custom-patches/typegoose.patch";
import "./patches/custom-patches/nestjs.patch";
import "reflect-metadata";
import { environments } from "./configs/app-config";
import { Passport } from "passport";
import { ApolloServer } from "apollo-server-express";
import { ValidationPipe, Logger } from "@nestjs/common";
import { NestFactory, INestApplication } from "@nestjs/core";
import { ServiceLocator } from "./shared/utils/service-locator";
import { AppModule } from "./app/app.module";
async function main() {
  const app = await NestFactory.create(AppModule);
  const globalPrefix = 'api';
  app.setGlobalPrefix(globalPrefix);
  app.useGlobalPipes(new ValidationPipe());
  ServiceLocator.config(app);
  await app.listen(4000);
  Logger.log(`Application is running on: ${await app.getUrl()}`);
}

main();
