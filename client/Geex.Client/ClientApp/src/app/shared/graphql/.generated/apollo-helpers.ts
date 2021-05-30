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
  | 'placeHolder'
  | 'createRole'
  | 'register'
  | 'assignRoles'
  | 'updateProfile'
  | 'generateCaptcha'
  | 'validateCaptcha'
  | 'authorize'
  | 'authenticate'
  | 'updateSetting'
  | MutationKeySpecifier
)[];
export type MutationFieldPolicy = {
  placeHolder?: FieldPolicy<any> | FieldReadFunction<any>;
  createRole?: FieldPolicy<any> | FieldReadFunction<any>;
  register?: FieldPolicy<any> | FieldReadFunction<any>;
  assignRoles?: FieldPolicy<any> | FieldReadFunction<any>;
  updateProfile?: FieldPolicy<any> | FieldReadFunction<any>;
  generateCaptcha?: FieldPolicy<any> | FieldReadFunction<any>;
  validateCaptcha?: FieldPolicy<any> | FieldReadFunction<any>;
  authorize?: FieldPolicy<any> | FieldReadFunction<any>;
  authenticate?: FieldPolicy<any> | FieldReadFunction<any>;
  updateSetting?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type QueryKeySpecifier = ('placeHolder' | 'queryRoles' | 'queryUsers' | 'userProfile' | 'settings' | QueryKeySpecifier)[];
export type QueryFieldPolicy = {
  placeHolder?: FieldPolicy<any> | FieldReadFunction<any>;
  queryRoles?: FieldPolicy<any> | FieldReadFunction<any>;
  queryUsers?: FieldPolicy<any> | FieldReadFunction<any>;
  userProfile?: FieldPolicy<any> | FieldReadFunction<any>;
  settings?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type RoleKeySpecifier = ('id' | 'name' | RoleKeySpecifier)[];
export type RoleFieldPolicy = {
  id?: FieldPolicy<any> | FieldReadFunction<any>;
  name?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type SettingKeySpecifier = ('scope' | 'scopedKey' | 'value' | 'name' | SettingKeySpecifier)[];
export type SettingFieldPolicy = {
  scope?: FieldPolicy<any> | FieldReadFunction<any>;
  scopedKey?: FieldPolicy<any> | FieldReadFunction<any>;
  value?: FieldPolicy<any> | FieldReadFunction<any>;
  name?: FieldPolicy<any> | FieldReadFunction<any>;
};
export type SubscriptionKeySpecifier = ('placeHolder' | SubscriptionKeySpecifier)[];
export type SubscriptionFieldPolicy = {
  placeHolder?: FieldPolicy<any> | FieldReadFunction<any>;
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
  Setting?: Omit<TypePolicy, 'fields' | 'keyFields'> & {
    keyFields?: false | SettingKeySpecifier | (() => undefined | SettingKeySpecifier);
    fields?: SettingFieldPolicy;
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
