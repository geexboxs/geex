import request from 'umi-request';
import { UserRegisterParams } from './index';
import { ApolloClientInstance } from '../../../services/client';
import { gql, FetchResult } from '@apollo/client';

export async function fakeRegister(params: UserRegisterParams) {
  let result = await ApolloClientInstance.mutate<string>({
    mutation: gql`mutation _{
      register(registerInput: {username:"${params.mobile ?? params.mail}",password:"${
      params.password
    }"})
    }`,
  });
  return result.data;
}
