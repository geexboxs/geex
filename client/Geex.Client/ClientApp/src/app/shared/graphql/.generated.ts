import { gql } from 'apollo-angular';
import { Injectable } from '@angular/core';
import * as Apollo from 'apollo-angular';
import { GraphQLModule } from 'src/app/shared/graphql/graphql.module';
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
  /** ^\[1\]\(\[3-9\]\)[0-9]{9}$ */
  ChinesePhoneNumberType: any;
  ObjectId: any;
};

export enum AppPermission {
  AssignRole = 'ASSIGN_ROLE',
}

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
  authorizeTargetType: AuthorizeTargetType;
  allowedPermissions: Array<AppPermission>;
  targetId: Scalars['ObjectId'];
};

export enum AuthorizeTargetType {
  Role = 'ROLE',
  User = 'USER',
}

export type Captcha = {
  __typename?: 'Captcha';
  captchaType: CaptchaType;
  key: Scalars['String'];
  bitmap?: Maybe<Scalars['String']>;
};

export enum CaptchaProvider {
  Image = 'IMAGE',
  Sms = 'SMS',
}

export enum CaptchaType {
  Number = 'NUMBER',
  English = 'ENGLISH',
  NumberAndLetter = 'NUMBER_AND_LETTER',
  Chinese = 'CHINESE',
}

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

export enum LoginProvider {
  Local = 'LOCAL',
}

export type Mutation = {
  __typename?: 'Mutation';
  placeHolder: Scalars['String'];
  createRole: Scalars['Boolean'];
  register: Scalars['Boolean'];
  assignRoles: Scalars['Boolean'];
  generateCaptcha: Captcha;
  validateCaptcha: Scalars['Boolean'];
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

export type MutationGenerateCaptchaArgs = {
  input: SendCaptchaInput;
};

export type MutationValidateCaptchaArgs = {
  input: ValidateCaptchaInput;
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

export type SendCaptchaInput = {
  captchaProvider: CaptchaProvider;
  smsCaptchaPhoneNumber?: Maybe<Scalars['ChinesePhoneNumberType']>;
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

export type ValidateCaptchaInput = {
  captchaKey: Scalars['String'];
  captchaProvider: CaptchaProvider;
  captchaCode: Scalars['String'];
};

export type GenerateCaptchaMutationVariables = Exact<{
  captchaProvider: CaptchaProvider;
  smsCaptchaPhoneNumber?: Maybe<Scalars['ChinesePhoneNumberType']>;
}>;

export type GenerateCaptchaMutation = { __typename?: 'Mutation' } & {
  generateCaptcha: { __typename?: 'Captcha' } & Pick<Captcha, 'bitmap' | 'key'>;
};

export type ValidateSmsCaptchaMutationVariables = Exact<{
  captchaKey: Scalars['String'];
  captchaCode: Scalars['String'];
}>;

export type ValidateSmsCaptchaMutation = { __typename?: 'Mutation' } & Pick<Mutation, 'validateCaptcha'>;

export type RegisterAndSignInMutationVariables = Exact<{
  phoneOrEmail: Scalars['String'];
  password: Scalars['String'];
}>;

export type RegisterAndSignInMutation = { __typename?: 'Mutation' } & Pick<Mutation, 'register'> & {
    authenticate: { __typename?: 'IdentityUserTokenOfString' } & Pick<IdentityUserTokenOfString, 'value'>;
  };

export type AuthenticateMutationVariables = Exact<{
  userIdentifier: Scalars['String'];
  password: Scalars['String'];
}>;

export type AuthenticateMutation = { __typename?: 'Mutation' } & {
  authenticate: { __typename?: 'IdentityUserTokenOfString' } & Pick<IdentityUserTokenOfString, 'value'>;
};

export const GenerateCaptchaDocument = gql`
  mutation generateCaptcha($captchaProvider: CaptchaProvider!, $smsCaptchaPhoneNumber: ChinesePhoneNumberType) {
    generateCaptcha(input: { captchaProvider: $captchaProvider, smsCaptchaPhoneNumber: $smsCaptchaPhoneNumber }) {
      bitmap
      key
    }
  }
`;

@Injectable({
  providedIn: GraphQLModule,
})
export class GenerateCaptchaGqlMutation extends Apollo.Mutation<GenerateCaptchaMutation, GenerateCaptchaMutationVariables> {
  document = GenerateCaptchaDocument;

  constructor(apollo: Apollo.Apollo) {
    super(apollo);
  }
}
export const ValidateSmsCaptchaDocument = gql`
  mutation validateSmsCaptcha($captchaKey: String!, $captchaCode: String!) {
    validateCaptcha(input: { captchaKey: $captchaKey, captchaProvider: SMS, captchaCode: $captchaCode })
  }
`;

@Injectable({
  providedIn: GraphQLModule,
})
export class ValidateSmsCaptchaGqlMutation extends Apollo.Mutation<ValidateSmsCaptchaMutation, ValidateSmsCaptchaMutationVariables> {
  document = ValidateSmsCaptchaDocument;

  constructor(apollo: Apollo.Apollo) {
    super(apollo);
  }
}
export const RegisterAndSignInDocument = gql`
  mutation registerAndSignIn($phoneOrEmail: String!, $password: String!) {
    register(input: { phoneOrEmail: $phoneOrEmail, password: $password, userName: $phoneOrEmail })
    authenticate(input: { userIdentifier: $phoneOrEmail, password: $password }) {
      value
    }
  }
`;

@Injectable({
  providedIn: GraphQLModule,
})
export class RegisterAndSignInGqlMutation extends Apollo.Mutation<RegisterAndSignInMutation, RegisterAndSignInMutationVariables> {
  document = RegisterAndSignInDocument;

  constructor(apollo: Apollo.Apollo) {
    super(apollo);
  }
}
export const AuthenticateDocument = gql`
  mutation authenticate($userIdentifier: String!, $password: String!) {
    authenticate(input: { userIdentifier: $userIdentifier, password: $password }) {
      value
    }
  }
`;

@Injectable({
  providedIn: GraphQLModule,
})
export class AuthenticateGqlMutation extends Apollo.Mutation<AuthenticateMutation, AuthenticateMutationVariables> {
  document = AuthenticateDocument;

  constructor(apollo: Apollo.Apollo) {
    super(apollo);
  }
}
