import { TypedDocumentNode as DocumentNode } from '@graphql-typed-document-node/core';
import gql from 'graphql-tag';
export type Maybe<T> = T | null;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
/** All built-in and custom scalars, mapped to their actual values */
export interface Scalars {
  ID: string;
  String: string;
  Boolean: boolean;
  Int: number;
  Float: number;
  /** ^\[1\]\(\[3-9\]\)[0-9]{9}$ */
  ChinesePhoneNumberType: any;
  ObjectId: any;
}

export type AppPermission = 'ASSIGN_ROLE';

export type AppSettings = 'APP_MENU' | 'APP_NAME';

export type ApplyPolicy = 'BEFORE_RESOLVER' | 'AFTER_RESOLVER';

export interface AssignRoleInput {
  userId: Scalars['ObjectId'];
  roles: Array<Scalars['String']>;
}

export interface AuthenticateInput {
  userIdentifier: Scalars['String'];
  password: Scalars['String'];
}

export interface AuthorizeInput {
  authorizeTargetType: AuthorizeTargetType;
  allowedPermissions: Array<AppPermission>;
  targetId: Scalars['ObjectId'];
}

export type AuthorizeTargetType = 'ROLE' | 'USER';

export interface Captcha {
  __typename?: 'Captcha';
  captchaType: CaptchaType;
  key: Scalars['String'];
  bitmap?: Maybe<Scalars['String']>;
}

export type CaptchaProvider = 'IMAGE' | 'SMS';

export type CaptchaType = 'NUMBER' | 'ENGLISH' | 'NUMBER_AND_LETTER' | 'CHINESE';

export interface ClaimsIdentity {
  __typename?: 'ClaimsIdentity';
  name?: Maybe<Scalars['String']>;
}

export interface CreateRoleInput {
  roleName: Scalars['String'];
}

export interface GetSettingsInput {
  scope: SettingScopeEnumeration;
}

export interface IUserProfile {
  __typename?: 'IUserProfile';
  avatar: Scalars['String'];
  userName: Scalars['String'];
}

export interface IdentityUserTokenOfString {
  __typename?: 'IdentityUserTokenOfString';
  userId?: Maybe<Scalars['String']>;
  loginProvider?: Maybe<Scalars['String']>;
  name?: Maybe<Scalars['String']>;
  value?: Maybe<Scalars['String']>;
}

export type LoginProvider = 'LOCAL';

export interface Mutation {
  __typename?: 'Mutation';
  placeHolder: Scalars['String'];
  createRole: Scalars['Boolean'];
  register: Scalars['Boolean'];
  assignRoles: Scalars['Boolean'];
  updateProfile: Scalars['Boolean'];
  generateCaptcha: Captcha;
  validateCaptcha: Scalars['Boolean'];
  authorize: Scalars['Boolean'];
  authenticate: IdentityUserTokenOfString;
  updateSetting: Scalars['Boolean'];
}

export interface MutationCreateRoleArgs {
  input: CreateRoleInput;
}

export interface MutationRegisterArgs {
  input: RegisterUserInput;
}

export interface MutationAssignRolesArgs {
  input: AssignRoleInput;
}

export interface MutationUpdateProfileArgs {
  input: UploadProfileInput;
}

export interface MutationGenerateCaptchaArgs {
  input: SendCaptchaInput;
}

export interface MutationValidateCaptchaArgs {
  input: ValidateCaptchaInput;
}

export interface MutationAuthorizeArgs {
  input: AuthorizeInput;
}

export interface MutationAuthenticateArgs {
  input: AuthenticateInput;
}

export interface MutationUpdateSettingArgs {
  input: UpdateSettingInput;
}

export interface Query {
  __typename?: 'Query';
  placeHolder: Scalars['String'];
  queryRoles: Array<Role>;
  /** This field does ... */
  queryUsers: Array<User>;
  userProfile: IUserProfile;
  settings: Array<Setting>;
}

export interface QueryQueryRolesArgs {
  input: CreateRoleInput;
}

export interface QueryUserProfileArgs {
  userIdentifier: Scalars['String'];
}

export interface QuerySettingsArgs {
  dto: GetSettingsInput;
}

export interface RegisterUserInput {
  userName: Scalars['String'];
  password: Scalars['String'];
  phoneOrEmail: Scalars['String'];
}

export interface Role {
  __typename?: 'Role';
  id: Scalars['String'];
  name: Scalars['String'];
}

export interface SendCaptchaInput {
  captchaProvider: CaptchaProvider;
  smsCaptchaPhoneNumber?: Maybe<Scalars['ChinesePhoneNumberType']>;
}

export interface Setting {
  __typename?: 'Setting';
  scope?: Maybe<SettingScopeEnumeration>;
  scopedKey?: Maybe<Scalars['String']>;
  value: Scalars['String'];
  name: SettingDefinition;
}

export type SettingDefinition = 'APP_MENU' | 'APP_NAME';

export type SettingScopeEnumeration = 'EFFECTIVE' | 'GLOBAL' | 'USER';

export interface Subscription {
  __typename?: 'Subscription';
  placeHolder: Scalars['String'];
}

export interface UpdateSettingInput {
  name: SettingDefinition;
  value?: Maybe<Scalars['String']>;
  scopedKey?: Maybe<Scalars['String']>;
  scope: SettingScopeEnumeration;
}

export interface UploadProfileInput {
  userId: Scalars['ObjectId'];
  newAvatar?: Maybe<Scalars['String']>;
}

export interface User {
  __typename?: 'User';
  id: Scalars['String'];
  roles?: Maybe<Array<Maybe<Role>>>;
}

export interface ValidateCaptchaInput {
  captchaKey: Scalars['String'];
  captchaProvider: CaptchaProvider;
  captchaCode: Scalars['String'];
}

export type SettingPairFragment = { __typename?: 'Setting' } & Pick<Setting, 'name' | 'value'>;

export type AuthenticateMutationVariables = Exact<{
  userIdentifier: Scalars['String'];
  password: Scalars['String'];
}>;

export type AuthenticateMutation = { __typename?: 'Mutation' } & {
  authenticate: { __typename?: 'IdentityUserTokenOfString' } & Pick<IdentityUserTokenOfString, 'value'>;
};

export type AssignRolesMutationVariables = Exact<{
  userId: Scalars['ObjectId'];
  roleId: Scalars['String'];
}>;

export type AssignRolesMutation = { __typename?: 'Mutation' } & Pick<Mutation, 'assignRoles'>;

export type RegisterAndSignInMutationVariables = Exact<{
  phoneOrEmail: Scalars['String'];
  password: Scalars['String'];
}>;

export type RegisterAndSignInMutation = { __typename?: 'Mutation' } & Pick<Mutation, 'register'> & {
    authenticate: { __typename?: 'IdentityUserTokenOfString' } & Pick<IdentityUserTokenOfString, 'value'>;
  };

export type UserProfileQueryVariables = Exact<{
  userIdentifier: Scalars['String'];
}>;

export type UserProfileQuery = { __typename?: 'Query' } & {
  userProfile: { __typename?: 'IUserProfile' } & Pick<IUserProfile, 'avatar' | 'userName'>;
};

export type UpdateProfileMutationVariables = Exact<{
  newAvatar?: Maybe<Scalars['String']>;
  userId: Scalars['ObjectId'];
}>;

export type UpdateProfileMutation = { __typename?: 'Mutation' } & Pick<Mutation, 'updateProfile'>;

export type SettingsQueryVariables = Exact<{ [key: string]: never }>;

export type SettingsQuery = { __typename?: 'Query' } & { settings: Array<{ __typename?: 'Setting' } & SettingPairFragment> };

export type UpdateSettingMutationVariables = Exact<{
  settingScope: SettingScopeEnumeration;
  settingName: SettingDefinition;
  settingValue?: Maybe<Scalars['String']>;
  settingScopeKey?: Maybe<Scalars['String']>;
}>;

export type UpdateSettingMutation = { __typename?: 'Mutation' } & Pick<Mutation, 'updateSetting'>;

export type SendSmsCaptchaMutationVariables = Exact<{
  phoneOrEmail: Scalars['ChinesePhoneNumberType'];
}>;

export type SendSmsCaptchaMutation = { __typename?: 'Mutation' } & {
  generateCaptcha: { __typename?: 'Captcha' } & Pick<Captcha, 'captchaType' | 'key'>;
};

export type ValidateSmsCaptchaMutationVariables = Exact<{
  captchaKey: Scalars['String'];
  captchaCode: Scalars['String'];
}>;

export type ValidateSmsCaptchaMutation = { __typename?: 'Mutation' } & Pick<Mutation, 'validateCaptcha'>;

export type InitSettingsQueryVariables = Exact<{ [key: string]: never }>;

export type InitSettingsQuery = { __typename?: 'Query' } & {
  settings: Array<{ __typename?: 'Setting' } & Pick<Setting, 'name' | 'value'>>;
};

export const SettingPairGql = (gql`
  fragment SettingPair on Setting {
    name
    value
  }
` as unknown) as DocumentNode<SettingPairFragment, unknown>;
export const AuthenticateGql = (gql`
  mutation authenticate($userIdentifier: String!, $password: String!) {
    authenticate(input: { userIdentifier: $userIdentifier, password: $password }) {
      value
    }
  }
` as unknown) as DocumentNode<AuthenticateMutation, AuthenticateMutationVariables>;
export const AssignRolesGql = (gql`
  mutation assignRoles($userId: ObjectId!, $roleId: String!) {
    assignRoles(input: { userId: $userId, roles: [$roleId] })
  }
` as unknown) as DocumentNode<AssignRolesMutation, AssignRolesMutationVariables>;
export const RegisterAndSignInGql = (gql`
  mutation registerAndSignIn($phoneOrEmail: String!, $password: String!) {
    register(input: { phoneOrEmail: $phoneOrEmail, password: $password, userName: $phoneOrEmail })
    authenticate(input: { userIdentifier: $phoneOrEmail, password: $password }) {
      value
    }
  }
` as unknown) as DocumentNode<RegisterAndSignInMutation, RegisterAndSignInMutationVariables>;
export const UserProfileGql = (gql`
  query userProfile($userIdentifier: String!) {
    userProfile(userIdentifier: $userIdentifier) {
      avatar
      userName
    }
  }
` as unknown) as DocumentNode<UserProfileQuery, UserProfileQueryVariables>;
export const UpdateProfileGql = (gql`
  mutation updateProfile($newAvatar: String, $userId: ObjectId!) {
    updateProfile(input: { newAvatar: $newAvatar, userId: $userId })
  }
` as unknown) as DocumentNode<UpdateProfileMutation, UpdateProfileMutationVariables>;
export const SettingsGql = (gql`
  query settings {
    settings(dto: { scope: GLOBAL }) {
      ...SettingPair
    }
  }
  ${SettingPairGql}
` as unknown) as DocumentNode<SettingsQuery, SettingsQueryVariables>;
export const UpdateSettingGql = (gql`
  mutation updateSetting(
    $settingScope: SettingScopeEnumeration!
    $settingName: SettingDefinition!
    $settingValue: String
    $settingScopeKey: String
  ) {
    updateSetting(input: { name: $settingName, scope: $settingScope, scopedKey: $settingScopeKey, value: $settingValue })
  }
` as unknown) as DocumentNode<UpdateSettingMutation, UpdateSettingMutationVariables>;
export const SendSmsCaptchaGql = (gql`
  mutation sendSmsCaptcha($phoneOrEmail: ChinesePhoneNumberType!) {
    generateCaptcha(input: { captchaProvider: SMS, smsCaptchaPhoneNumber: $phoneOrEmail }) {
      captchaType
      key
    }
  }
` as unknown) as DocumentNode<SendSmsCaptchaMutation, SendSmsCaptchaMutationVariables>;
export const ValidateSmsCaptchaGql = (gql`
  mutation validateSmsCaptcha($captchaKey: String!, $captchaCode: String!) {
    validateCaptcha(input: { captchaProvider: SMS, captchaKey: $captchaKey, captchaCode: $captchaCode })
  }
` as unknown) as DocumentNode<ValidateSmsCaptchaMutation, ValidateSmsCaptchaMutationVariables>;
export const InitSettingsGql = (gql`
  query initSettings {
    settings(dto: { scope: EFFECTIVE }) {
      name
      value
    }
  }
` as unknown) as DocumentNode<InitSettingsQuery, InitSettingsQueryVariables>;
