import { Model, Document } from "mongoose";
import { ModelType, InstanceType } from "typegoose";
import { ResolverInterface } from "type-graphql";

export type IResolver<T> = Omit<ResolverInterface<InstanceType<T>>, keyof Document>;

