import { gql } from 'apollo-angular';
export type Maybe<T> = T | null;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: string;
  String: string;
  Boolean: boolean;
  Int: number;
  Float: number;
  AppPermission: any;
  AuthorizeTargetType: any;
  LoginProvider: any;
  ObjectId: any;
};

export enum ApplyPolicy {
  BeforeResolver = 'BEFORE_RESOLVER',
  AfterResolver = 'AFTER_RESOLVER',
}

export type AssignRoleInput = {
  userId: Scalars['ObjectId'];
  roles: Array<Scalars['String']>;
};

export type AuthenticateInput = {
  userIdentifier: Scalars['String'];
  password: Scalars['String'];
};

export type AuthorizeInput = {
  authorizeTargetType: Scalars['AuthorizeTargetType'];
  allowedPermissions: Array<Scalars['AppPermission']>;
  targetId: Scalars['ObjectId'];
};

export type ClaimsIdentity = {
  __typename?: 'ClaimsIdentity';
  name?: Maybe<Scalars['String']>;
};

export type CreateRoleInput = {
  roleName: Scalars['String'];
};

export type IdentityUserTokenOfString = {
  __typename?: 'IdentityUserTokenOfString';
  userId?: Maybe<Scalars['String']>;
  loginProvider?: Maybe<Scalars['String']>;
  name?: Maybe<Scalars['String']>;
  value?: Maybe<Scalars['String']>;
};

export type Mutation = {
  __typename?: 'Mutation';
  placeHolder: Scalars['String'];
  createRole: Scalars['Boolean'];
  register: Scalars['Boolean'];
  assignRoles: Scalars['Boolean'];
  authorize: Scalars['Boolean'];
  authenticate: IdentityUserTokenOfString;
};

export type MutationCreateRoleArgs = {
  input: CreateRoleInput;
};

export type MutationRegisterArgs = {
  input: RegisterUserInput;
};

export type MutationAssignRolesArgs = {
  input: AssignRoleInput;
};

export type MutationAuthorizeArgs = {
  input: AuthorizeInput;
};

export type MutationAuthenticateArgs = {
  input: AuthenticateInput;
};

export type Query = {
  __typename?: 'Query';
  placeHolder: Scalars['String'];
  queryRoles: Array<Role>;
  /** This field does ... */
  queryUsers: Array<User>;
};

export type QueryQueryRolesArgs = {
  input: CreateRoleInput;
};

export type RegisterUserInput = {
  userName: Scalars['String'];
  password: Scalars['String'];
  phoneOrEmail: Scalars['String'];
};

export type Role = {
  __typename?: 'Role';
  id: Scalars['String'];
  name: Scalars['String'];
};

export type Subscription = {
  __typename?: 'Subscription';
  placeHolder: Scalars['String'];
};

export type User = {
  __typename?: 'User';
  id: Scalars['String'];
  roles?: Maybe<Array<Maybe<Role>>>;
};
