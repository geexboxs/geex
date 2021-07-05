import { FieldPolicy, FieldReadFunction, TypePolicies, TypePolicy } from '@apollo/client/cache';
export type CaptchaKeySpecifier = ('captchaType' | 'key' | 'bitmap' | CaptchaKeySpecifier)[];
export type CaptchaFieldPolicy = {
  captchaType?: FieldPolicy<any> | FieldReadFunction<any>;
  key?: FieldPolicy<any> | FieldReadFunction<any>;
  bitmap?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type ClaimsIdentityKeySpecifier = ('name' | ClaimsIdentityKeySpecifier)[];
export type ClaimsIdentityFieldPolicy = {
  name?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type IFrontendCallKeySpecifier = ('data' | 'frontendCallType' | IFrontendCallKeySpecifier)[];
export type IFrontendCallFieldPolicy = {
  data?: FieldPolicy<any> | FieldReadFunction<any>;
  frontendCallType?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type IMessageKeySpecifier = ('fromUserId' | 'messageType' | 'content' | 'toUserIds' | 'id' | 'severity' | IMessageKeySpecifier)[];
export type IMessageFieldPolicy = {
  fromUserId?: FieldPolicy<any> | FieldReadFunction<any>;
  messageType?: FieldPolicy<any> | FieldReadFunction<any>;
  content?: FieldPolicy<any> | FieldReadFunction<any>;
  toUserIds?: FieldPolicy<any> | FieldReadFunction<any>;
  id?: FieldPolicy<any> | FieldReadFunction<any>;
  severity?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type IMessageContentKeySpecifier = ('title' | 'time' | IMessageContentKeySpecifier)[];
export type IMessageContentFieldPolicy = {
  title?: FieldPolicy<any> | FieldReadFunction<any>;
  time?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type ISettingKeySpecifier = ('scope' | 'scopedKey' | 'value' | 'name' | 'id' | ISettingKeySpecifier)[];
export type ISettingFieldPolicy = {
  scope?: FieldPolicy<any> | FieldReadFunction<any>;
  scopedKey?: FieldPolicy<any> | FieldReadFunction<any>;
  value?: FieldPolicy<any> | FieldReadFunction<any>;
  name?: FieldPolicy<any> | FieldReadFunction<any>;
  id?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type ITestKeySpecifier = ('name' | 'data' | ITestKeySpecifier)[];
export type ITestFieldPolicy = {
  name?: FieldPolicy<any> | FieldReadFunction<any>;
  data?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type ITestTemplateKeySpecifier = ('name' | ITestTemplateKeySpecifier)[];
export type ITestTemplateFieldPolicy = {
  name?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type IUserProfileKeySpecifier = ('avatar' | 'userName' | IUserProfileKeySpecifier)[];
export type IUserProfileFieldPolicy = {
  avatar?: FieldPolicy<any> | FieldReadFunction<any>;
  userName?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type IdentityUserTokenOfStringKeySpecifier = (
  | 'userId'
  | 'loginProvider'
  | 'name'
  | 'value'
  | IdentityUserTokenOfStringKeySpecifier
)[];
export type IdentityUserTokenOfStringFieldPolicy = {
  userId?: FieldPolicy<any> | FieldReadFunction<any>;
  loginProvider?: FieldPolicy<any> | FieldReadFunction<any>;
  name?: FieldPolicy<any> | FieldReadFunction<any>;
  value?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type MutationKeySpecifier = (
  | '_'
  | 'markMessagesRead'
  | 'deleteMessageDistributions'
  | 'sendMessage'
  | 'updateSetting'
  | 'createRole'
  | 'register'
  | 'assignRoles'
  | 'updateProfile'
  | 'updateTestTemplate'
  | 'updateTest'
  | 'generateCaptcha'
  | 'validateCaptcha'
  | 'authorize'
  | 'authenticate'
  | MutationKeySpecifier
)[];
export type MutationFieldPolicy = {
  _?: FieldPolicy<any> | FieldReadFunction<any>;
  markMessagesRead?: FieldPolicy<any> | FieldReadFunction<any>;
  deleteMessageDistributions?: FieldPolicy<any> | FieldReadFunction<any>;
  sendMessage?: FieldPolicy<any> | FieldReadFunction<any>;
  updateSetting?: FieldPolicy<any> | FieldReadFunction<any>;
  createRole?: FieldPolicy<any> | FieldReadFunction<any>;
  register?: FieldPolicy<any> | FieldReadFunction<any>;
  assignRoles?: FieldPolicy<any> | FieldReadFunction<any>;
  updateProfile?: FieldPolicy<any> | FieldReadFunction<any>;
  updateTestTemplate?: FieldPolicy<any> | FieldReadFunction<any>;
  updateTest?: FieldPolicy<any> | FieldReadFunction<any>;
  generateCaptcha?: FieldPolicy<any> | FieldReadFunction<any>;
  validateCaptcha?: FieldPolicy<any> | FieldReadFunction<any>;
  authorize?: FieldPolicy<any> | FieldReadFunction<any>;
  authenticate?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type QueryKeySpecifier = (
  | '_'
  | 'messages'
  | 'unreadMessages'
  | 'settings'
  | 'queryRoles'
  | 'queryUsers'
  | 'userProfile'
  | 'testTemplates'
  | 'tests'
  | QueryKeySpecifier
)[];
export type QueryFieldPolicy = {
  _?: FieldPolicy<any> | FieldReadFunction<any>;
  messages?: FieldPolicy<any> | FieldReadFunction<any>;
  unreadMessages?: FieldPolicy<any> | FieldReadFunction<any>;
  settings?: FieldPolicy<any> | FieldReadFunction<any>;
  queryRoles?: FieldPolicy<any> | FieldReadFunction<any>;
  queryUsers?: FieldPolicy<any> | FieldReadFunction<any>;
  userProfile?: FieldPolicy<any> | FieldReadFunction<any>;
  testTemplates?: FieldPolicy<any> | FieldReadFunction<any>;
  tests?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type RoleKeySpecifier = ('id' | 'name' | RoleKeySpecifier)[];
export type RoleFieldPolicy = {
  id?: FieldPolicy<any> | FieldReadFunction<any>;
  name?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type SubscriptionKeySpecifier = ('_' | 'onFrontendCall' | SubscriptionKeySpecifier)[];
export type SubscriptionFieldPolicy = {
  _?: FieldPolicy<any> | FieldReadFunction<any>;
  onFrontendCall?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type UserKeySpecifier = ('id' | 'roles' | UserKeySpecifier)[];
export type UserFieldPolicy = {
  id?: FieldPolicy<any> | FieldReadFunction<any>;
  roles?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type TypedTypePolicies = TypePolicies & {
  Captcha?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | CaptchaKeySpecifier | (() => undefined | CaptchaKeySpecifier);
    fields?: CaptchaFieldPolicy;
  };
  ClaimsIdentity?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | ClaimsIdentityKeySpecifier | (() => undefined | ClaimsIdentityKeySpecifier);
    fields?: ClaimsIdentityFieldPolicy;
  };
  IFrontendCall?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | IFrontendCallKeySpecifier | (() => undefined | IFrontendCallKeySpecifier);
    fields?: IFrontendCallFieldPolicy;
  };
  IMessage?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | IMessageKeySpecifier | (() => undefined | IMessageKeySpecifier);
    fields?: IMessageFieldPolicy;
  };
  IMessageContent?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | IMessageContentKeySpecifier | (() => undefined | IMessageContentKeySpecifier);
    fields?: IMessageContentFieldPolicy;
  };
  ISetting?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | ISettingKeySpecifier | (() => undefined | ISettingKeySpecifier);
    fields?: ISettingFieldPolicy;
  };
  ITest?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | ITestKeySpecifier | (() => undefined | ITestKeySpecifier);
    fields?: ITestFieldPolicy;
  };
  ITestTemplate?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | ITestTemplateKeySpecifier | (() => undefined | ITestTemplateKeySpecifier);
    fields?: ITestTemplateFieldPolicy;
  };
  IUserProfile?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | IUserProfileKeySpecifier | (() => undefined | IUserProfileKeySpecifier);
    fields?: IUserProfileFieldPolicy;
  };
  IdentityUserTokenOfString?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | IdentityUserTokenOfStringKeySpecifier | (() => undefined | IdentityUserTokenOfStringKeySpecifier);
    fields?: IdentityUserTokenOfStringFieldPolicy;
  };
  Mutation?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | MutationKeySpecifier | (() => undefined | MutationKeySpecifier);
    fields?: MutationFieldPolicy;
  };
  Query?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | QueryKeySpecifier | (() => undefined | QueryKeySpecifier);
    fields?: QueryFieldPolicy;
  };
  Role?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | RoleKeySpecifier | (() => undefined | RoleKeySpecifier);
    fields?: RoleFieldPolicy;
  };
  Subscription?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | SubscriptionKeySpecifier | (() => undefined | SubscriptionKeySpecifier);
    fields?: SubscriptionFieldPolicy;
  };
  User?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | UserKeySpecifier | (() => undefined | UserKeySpecifier);
    fields?: UserFieldPolicy;
  };
};
