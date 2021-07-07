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
  Any: any;
  /** ^\[1\]\(\[3-9\]\)[0-9]{9}$ */
  ChinesePhoneNumberType: any;
  /** The `DateTime` scalar represents an ISO-8601 compliant date time type. */
  DateTime: any;
  ObjectId: any;
}

export type AppPermission = 'ASSIGN_ROLE';

export type AppSettings = 'APP_APP_MENU' | 'APP_APP_NAME' | 'LOCALIZATION_DATA' | 'LOCALIZATION_LANGUAGE' | 'TESTING_MODULE_NAME';

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

export interface DeleteMessageDistributionsInput {
  messageId?: Maybe<Scalars['String']>;
  userIds?: Maybe<Array<Maybe<Scalars['String']>>>;
}

export type FrontendCallType = 'NEW_MESSAGE';

export type GeexClaimType = 'AVATAR' | 'EXPIRES' | 'NICKNAME' | 'PROVIDER' | 'SUB';

export type GeexExceptionType = 'CONFLICT' | 'NOT_FOUND' | 'ON_PURPOSE' | 'UNKNOWN';

export interface GetMessagesInput {
  messageType?: Maybe<MessageType>;
}

export interface GetSettingsInput {
  scope?: Maybe<SettingScopeEnumeration>;
}

export interface GetTestTemplatesInput {
  name: Scalars['String'];
}

export interface GetTestsInput {
  name: Scalars['String'];
}

export interface GetUnreadMessagesInput {
  _?: Maybe<Scalars['String']>;
}

export interface IFrontendCall {
  __typename?: 'IFrontendCall';
  data?: Maybe<Scalars['Any']>;
  frontendCallType?: Maybe<FrontendCallType>;
}

export interface IMessage {
  __typename?: 'IMessage';
  fromUserId?: Maybe<Scalars['String']>;
  messageType: MessageType;
  content?: Maybe<IMessageContent>;
  toUserIds?: Maybe<Array<Maybe<Scalars['String']>>>;
  id?: Maybe<Scalars['String']>;
  severity: MessageSeverityType;
}

export interface IMessageContent {
  __typename?: 'IMessageContent';
  title?: Maybe<Scalars['String']>;
  time: Scalars['DateTime'];
}

export interface ISetting {
  __typename?: 'ISetting';
  scope?: Maybe<SettingScopeEnumeration>;
  scopedKey?: Maybe<Scalars['String']>;
  value?: Maybe<Scalars['String']>;
  name?: Maybe<SettingDefinition>;
  id?: Maybe<Scalars['String']>;
}

export interface ITest {
  __typename?: 'ITest';
  name: Scalars['String'];
  data: Scalars['String'];
}

export interface ITestTemplate {
  __typename?: 'ITestTemplate';
  name: Scalars['String'];
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

export type LocalizationSettings = 'APP_APP_MENU' | 'APP_APP_NAME' | 'LOCALIZATION_DATA' | 'LOCALIZATION_LANGUAGE' | 'TESTING_MODULE_NAME';

export type LoginProvider = 'LOCAL';

export interface MarkMessagesReadInput {
  messageIds?: Maybe<Array<Maybe<Scalars['String']>>>;
  userId?: Maybe<Scalars['String']>;
}

export type MessageSeverityType = 'INFO' | 'SUCCESS' | 'WARN' | 'ERROR' | 'FATAL';

export type MessageType = 'NOTIFICATION' | 'TODO' | 'INTERACT';

export type MessagingSettings = 'APP_APP_MENU' | 'APP_APP_NAME' | 'LOCALIZATION_DATA' | 'LOCALIZATION_LANGUAGE' | 'TESTING_MODULE_NAME';

export interface Mutation {
  __typename?: 'Mutation';
  _: Scalars['String'];
  markMessagesRead: Scalars['Boolean'];
  deleteMessageDistributions: Scalars['Boolean'];
  sendMessage: Scalars['Boolean'];
  updateSetting?: Maybe<ISetting>;
  createRole: Scalars['Boolean'];
  register: Scalars['Boolean'];
  assignRoles: Scalars['Boolean'];
  updateProfile: Scalars['Boolean'];
  updateTestTemplate: ITestTemplate;
  updateTest: ITest;
  generateCaptcha: Captcha;
  validateCaptcha: Scalars['Boolean'];
  authorize: Scalars['Boolean'];
  authenticate: IdentityUserTokenOfString;
}

export interface MutationMarkMessagesReadArgs {
  input?: Maybe<MarkMessagesReadInput>;
}

export interface MutationDeleteMessageDistributionsArgs {
  input?: Maybe<DeleteMessageDistributionsInput>;
}

export interface MutationSendMessageArgs {
  input?: Maybe<SendNotificationMessageRequestInput>;
}

export interface MutationUpdateSettingArgs {
  input?: Maybe<UpdateSettingInput>;
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

export interface MutationUpdateTestTemplateArgs {
  input: UpdateTestTemplateInput;
}

export interface MutationUpdateTestArgs {
  input: UpdateTestInput;
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

export interface Query {
  __typename?: 'Query';
  _: Scalars['String'];
  messages?: Maybe<Array<Maybe<IMessage>>>;
  unreadMessages?: Maybe<Array<Maybe<IMessage>>>;
  settings?: Maybe<Array<Maybe<ISetting>>>;
  queryRoles: Array<Role>;
  /** This field does ... */
  queryUsers: Array<User>;
  userProfile: IUserProfile;
  testTemplates: Array<ITestTemplate>;
  tests: Array<ITest>;
}

export interface QueryMessagesArgs {
  input?: Maybe<GetMessagesInput>;
}

export interface QueryUnreadMessagesArgs {
  input?: Maybe<GetUnreadMessagesInput>;
}

export interface QuerySettingsArgs {
  input?: Maybe<GetSettingsInput>;
}

export interface QueryQueryRolesArgs {
  input: CreateRoleInput;
}

export interface QueryUserProfileArgs {
  userIdentifier: Scalars['String'];
}

export interface QueryTestTemplatesArgs {
  input: GetTestTemplatesInput;
}

export interface QueryTestsArgs {
  input: GetTestsInput;
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

export interface SendNotificationMessageRequestInput {
  toUserIds?: Maybe<Array<Maybe<Scalars['String']>>>;
  messageContent?: Maybe<Scalars['String']>;
  severity: MessageSeverityType;
}

export type SettingDefinition = 'APP_APP_MENU' | 'APP_APP_NAME' | 'LOCALIZATION_DATA' | 'LOCALIZATION_LANGUAGE' | 'TESTING_MODULE_NAME';

export type SettingScopeEnumeration = 'GLOBAL' | 'USER';

export interface Subscription {
  __typename?: 'Subscription';
  _: Scalars['String'];
  onFrontendCall?: Maybe<IFrontendCall>;
}

export type TestingSettings = 'APP_APP_MENU' | 'APP_APP_NAME' | 'LOCALIZATION_DATA' | 'LOCALIZATION_LANGUAGE' | 'TESTING_MODULE_NAME';

export interface UpdateSettingInput {
  name?: Maybe<SettingDefinition>;
  value?: Maybe<Scalars['String']>;
  scopedKey?: Maybe<Scalars['String']>;
  scope?: Maybe<SettingScopeEnumeration>;
}

export interface UpdateTestInput {
  name: Scalars['String'];
  newName: Scalars['String'];
}

export interface UpdateTestTemplateInput {
  name: Scalars['String'];
  newName: Scalars['String'];
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

export type SettingPairFragment = { __typename?: 'ISetting' } & Pick<ISetting, 'name' | 'value'>;

export type MessageContentFragFragment = { __typename?: 'IMessageContent' } & Pick<IMessageContent, 'title' | 'time'>;

export type MessageFragFragment = { __typename?: 'IMessage' } & Pick<IMessage, 'fromUserId' | 'id' | 'messageType' | 'severity'> & {
    content?: Maybe<{ __typename?: 'IMessageContent' } & MessageContentFragFragment>;
  };

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

export type SettingsQuery = { __typename?: 'Query' } & {
  settings?: Maybe<Array<Maybe<{ __typename?: 'ISetting' } & SettingPairFragment>>>;
};

export type UpdateSettingMutationVariables = Exact<{
  settingScope: SettingScopeEnumeration;
  settingName: SettingDefinition;
  settingValue?: Maybe<Scalars['String']>;
  settingScopeKey?: Maybe<Scalars['String']>;
}>;

export type UpdateSettingMutation = { __typename?: 'Mutation' } & {
  updateSetting?: Maybe<{ __typename?: 'ISetting' } & Pick<ISetting, 'name' | 'value'>>;
};

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
  settings?: Maybe<Array<Maybe<{ __typename?: 'ISetting' } & Pick<ISetting, 'name' | 'value'>>>>;
};

export type OnFrontendCallSubscriptionVariables = Exact<{ [key: string]: never }>;

export type OnFrontendCallSubscription = { __typename?: 'Subscription' } & {
  onFrontendCall?: Maybe<{ __typename?: 'IFrontendCall' } & Pick<IFrontendCall, 'frontendCallType'>>;
};

export type MesagesQueryVariables = Exact<{ [key: string]: never }>;

export type MesagesQuery = { __typename?: 'Query' } & {
  messages?: Maybe<Array<Maybe<{ __typename?: 'IMessage' } & Pick<IMessage, 'toUserIds'> & MessageFragFragment>>>;
};

export type SendMessageMutationVariables = Exact<{
  toUserId: Scalars['String'];
  messageContent: Scalars['String'];
}>;

export type SendMessageMutation = { __typename?: 'Mutation' } & Pick<Mutation, 'sendMessage'>;

export const SettingPairGql = (gql`
  fragment SettingPair on ISetting {
    name
    value
  }
` as unknown) as DocumentNode<SettingPairFragment, unknown>;
export const MessageContentFragGql = (gql`
  fragment MessageContentFrag on IMessageContent {
    title
    time
  }
` as unknown) as DocumentNode<MessageContentFragFragment, unknown>;
export const MessageFragGql = (gql`
  fragment MessageFrag on IMessage {
    content {
      ...MessageContentFrag
    }
    fromUserId
    id
    messageType
    severity
  }
  ${MessageContentFragGql}
` as unknown) as DocumentNode<MessageFragFragment, unknown>;
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
    settings(input: { scope: GLOBAL }) {
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
    updateSetting(input: { name: $settingName, scope: $settingScope, scopedKey: $settingScopeKey, value: $settingValue }) {
      name
      value
    }
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
    settings(input: {}) {
      name
      value
    }
  }
` as unknown) as DocumentNode<InitSettingsQuery, InitSettingsQueryVariables>;
export const OnFrontendCallGql = (gql`
  subscription onFrontendCall {
    onFrontendCall {
      frontendCallType
    }
  }
` as unknown) as DocumentNode<OnFrontendCallSubscription, OnFrontendCallSubscriptionVariables>;
export const MesagesGql = (gql`
  query mesages {
    messages(input: {}) {
      ...MessageFrag
      toUserIds
    }
  }
  ${MessageFragGql}
` as unknown) as DocumentNode<MesagesQuery, MesagesQueryVariables>;
export const SendMessageGql = (gql`
  mutation sendMessage($toUserId: String!, $messageContent: String!) {
    sendMessage(input: { toUserIds: [$toUserId], severity: INFO, messageContent: $messageContent })
  }
` as unknown) as DocumentNode<SendMessageMutation, SendMessageMutationVariables>;
