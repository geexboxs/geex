import { GraphQLExtension } from "graphql-extensions";
import { IGeexRequestStart, IGeexContext } from "../../types";
import { getComplexity, simpleEstimator, fieldExtensionsEstimator } from "graphql-query-complexity";
import { request } from "http";
import { separateOperations, ExecutionArgs } from "graphql";

export class ComplexityExtension implements GraphQLExtension<IGeexContext> {
    public executionDidStart?({ executionArgs }: { executionArgs: ExecutionArgs; }) {
        const complexity = getComplexity({
            // Our built schema
            schema: executionArgs.schema,
            // To calculate query complexity properly,
            // we have to check if the document contains multiple operations
            // and eventually extract it operation from the whole query document.
            query: executionArgs.operationName
                ? separateOperations(executionArgs.document!)[executionArgs.operationName]
                : executionArgs.document!,
            // The variables for our GraphQL query
            variables: executionArgs.variableValues || {},
            // Add any number of estimators. The estimators are invoked in order, the first
            // numeric value that is being returned by an estimator is used as the field complexity.
            // If no estimator returns a value, an exception is raised.
            estimators: [
                // Using fieldExtensionsEstimator is mandatory to make it work with type-graphql.
                fieldExtensionsEstimator(),
                // Add more estimators here...
                // This will assign each field a complexity of 1
                // if no other estimator returned a value.
                simpleEstimator({ defaultComplexity: 1 }),
            ],
        });
        // Here we can react to the calculated complexity,
        // like compare it with max and throw error when the threshold is reached.
        if (complexity >= 20) {
            throw new Error(
                `Sorry, too complicated query! ${complexity} is over 20 that is the max allowed complexity.`,
            );
        }
    }
}
