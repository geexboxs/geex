import { NgModule } from '@angular/core';
import { APOLLO_OPTIONS } from 'apollo-angular';
import { ApolloClientOptions, InMemoryCache, concat, ApolloLink } from '@apollo/client/core';
import { HttpLink } from 'apollo-angular/http';
import { TypedTypePolicies } from 'src/app/shared/graphql/.generated/apollo-helpers';
import { LoadingService } from '../services/loading.service';

const typePolicies: TypedTypePolicies = {
  // Keys in this object will be validated against the typed on your schema
  User: {
    keyFields: ['id'],
  },
};

const uri = 'http://localhost:8000/graphql'; // <-- add the URL of the GraphQL server here
export function createApollo(httpLink: HttpLink, loadingSrv: LoadingService): ApolloClientOptions<any> {
  return {
    link: concat(
      new ApolloLink((operation, forward) => {
        loadingSrv.$loading.next(true);
        return forward(operation).map((response) => {
          loadingSrv.$loading.next(false);
          return response;
        });
      }),
      httpLink.create({ uri, withCredentials: true }),
    ),
    cache: new InMemoryCache({
      typePolicies: typePolicies,
    }),
    defaultOptions: {
      query: {
        fetchPolicy: 'no-cache',
      },
      watchQuery: {
        fetchPolicy: 'cache-first',
      },
    },
  };
}

@NgModule({
  providers: [
    {
      provide: APOLLO_OPTIONS,
      useFactory: createApollo,
      deps: [HttpLink, LoadingService],
    },
  ],
})
export class GraphQLModule {}
