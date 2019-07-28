import express = require("express");

import bodyParser = require("body-parser");

import { globalLoggingMiddleware } from "../shared/logging/globalLoggingMiddleware";

import accountsBoost from "@accounts/boost";

import { ApolloServer } from "apollo-server-express";

import { mergeTypeDefs, mergeResolvers } from "graphql-toolkit";

import { AppModule } from "../app/app.module";
