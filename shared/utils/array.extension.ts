import { List } from "linqts-camelcase";
declare global {
    interface ReadonlyArray<T> { 
        any(): boolean;
        any(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): boolean;
        any(predicate?: any);
    }
    interface Array<T> {
        // _elements: T[];
        add(element: T): void;
        // addRange(elements: T[]): void;
        // // aggregate<U>(accumulator: (accum: U, value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => any, initialValue?: U | undefined);
        // all(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): boolean;
        any(): boolean;
        any(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): boolean;
        any(predicate?: any);
        // average(): number;
        // average(transform: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => any): number;
        // average(transform?: any);
        contains(element: T): boolean;
        // count(): number;
        // count(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): number;
        // count(predicate?: any);
        // defaultIfEmpty(defaultValue?: T | undefined): List<T>;
        // distinct(): List<T>;
        // distinctBy(keySelector: (key: T) => string | number): List<T>;
        // elementAt(index: number): T;
        // elementAtOrDefault(index: number): T;
        // except(source: List<T>): List<T>;
        // first(): T;
        // first(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): T;
        // first(predicate?: any);
        // firstOrDefault(): T;
        // firstOrDefault(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): T;
        // firstOrDefault(predicate?: any);
        // forEach(action: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => any): void;
        // groupBy<TResult = T>(grouper: (key: T) => string | number, mapper?: ((element: T) => TResult) | undefined): { [key: string]: TResult[]; };
        // groupJoin<U>(list: List<U>, key1: (k: T) => any, key2: (k: U) => any, result: (first: T, second: List<U>) => any): List<any>;
        // indexOf(element: T): number;
        // insert(index: number, element: T): void | Error;
        intersect(source: Array<T>): Array<T>;
        // linqJoin<U>(list: List<U>, key1: (key: T) => any, key2: (key: U) => any, result: (first: T, second: U) => any): List<any>;
        // last(): T;
        // last(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): T;
        // last(predicate?: any);
        // lastOrDefault(): T;
        // lastOrDefault(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): T;
        // lastOrDefault(predicate?: any);
        // max(): number;
        // max(selector: (value: T, index: number, array: T[]) => number): number;
        // max(selector?: any);
        // min(): number;
        // min(selector: (value: T, index: number, array: T[]) => number): number;
        // min(selector?: any);
        // ofType<U>($type: any): List<U>;
        // orderBy(keySelector: (key: T) => any, comparer?: ((a: T, b: T) => number) | undefined): List<T>;
        // orderByDescending(keySelector: (key: T) => any, comparer?: ((a: T, b: T) => number) | undefined): List<T>;
        // thenBy(keySelector: (key: T) => any): List<T>;
        // thenByDescending(keySelector: (key: T) => any): List<T>;
        // remove(element: T): boolean;
        // removeAll(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): List<T>;
        // removeAt(index: number): void;
        // select<TOut>(selector: (element: T, index: number) => TOut): List<TOut>;
        // selectMany<TOut extends List<any>>(selector: (element: T, index: number) => TOut): TOut;
        // sequenceEqual(list: List<T>): boolean;
        // single(predicate?: ((value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean) | undefined): T;
        // singleOrDefault(predicate?: ((value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean) | undefined): T;
        // skip(amount: number): List<T>;
        // skipWhile(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): List<T>;
        // sum(): number;
        // sum(transform: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => number): number;
        // sum(transform?: any);
        // take(amount: number): List<T>;
        // takeWhile(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): List<T>;
        // toArray(): T[];
        // toDictionary<TKey>(key: (key: T) => TKey): List<{ Key: TKey; Value: T; }>;
        // toDictionary<TKey, TValue>(key: (key: T) => TKey, value: (value: T) => TValue): List<{ Key: TKey; Value: T | TValue; }>;
        // toDictionary(key: any, value?: any);
        // toList(): List<T>;
        // toLookup<TResult>(keySelector: (key: T) => string | number, elementSelector: (element: T) => TResult): { [key: string]: TResult[]; };
        // union(list: List<T>): List<T>;
        where(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): Array<T>;
        // zip<U, TOut>(list: List<U>, result: (first: T, second: U) => TOut): List<TOut>;
    }
}
// Array.prototype._elements = Array.prototype;
Array.prototype.add = function add<T>(this: Array<T>, element: T): void {
    this["_elements"] = this;
    return List.prototype.add.bind(this, element)();
}
// Array.prototype.addRange = function addRange<T>(this: Array<T>, elements: T[]): void {
//     return List.prototype.addRange.bind(this, elements)();
// }
// // Array.prototype.aggregate = function aggregate<T, U>(this: Array<T>, accumulator: (accum: U, value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => any, initialValue?: U | undefined) {
// //     List.prototype.addRange.bind(this, accumulator, initialValue);
// // }
// Array.prototype.all = function all<T>(this: Array<T>, predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): boolean {
//     return List.prototype.all.bind(this, predicate)();
// }
Array.prototype.any = function any<T>(this: Array<T>, predicate?: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): boolean {
    if (this === undefined) {
        return false;
    }
    this["_elements"] = this;
    return List.prototype.any.bind(this, predicate!)();
}
// Array.prototype.average = function average<T>(this: Array<T>, transform?: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => any): number {
//     return List.prototype.average.bind(this, transform!)();
// }
// Array.prototype.cast = function cast<U>(): List<U> {
//     return List.prototype.cast.bind(this)();
// }

Array.prototype.contains = function contains<T>(element: T): boolean {
    this["_elements"] = this;
    return List.prototype.contains.bind(this, element)();
}
// Array.prototype.count = function count<T>(this: Array<T>, predicate?: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): number {
//     return List.prototype.count.bind(this, element)();
// }
// Array.prototype.defaultIfEmpty = function defaultIfEmpty<T>(this: Array<T>, defaultValue?: T | undefined): List<T> {
//     return List.prototype.defaultIfEmpty.bind(this, element)();
// }
// Array.prototype.distinct = function distinct<T>(this: Array<T>, ): List<T> {
//     return List.prototype.distinct.bind(this, element)();
// }
// Array.prototype.distinctBy = function distinctBy<T>(this: Array<T>, keySelector: (key: T) => string | number): List<T> {
//     return List.prototype.distinctBy.bind(this, element)();
// }
// Array.prototype.elementAt = function elementAt<T>(this: Array<T>, index: number): T {
//     return List.prototype.elementAt.bind(this, element)();
// }
// Array.prototype.elementAtOrDefault = function elementAtOrDefault<T>(this: Array<T>, index: number): T {
//     return List.prototype.elementAtOrDefault.bind(this, element)();
// }
// Array.prototype.except = function except<T>(this: Array<T>, source: List<T>): List<T> {
//     return List.prototype.except.bind(this, element)();
// }
// Array.prototype.first = function first<T>(this: Array<T>, predicate?: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): T {
//     return List.prototype.first.bind(this, element)();
// }
// Array.prototype.firstOrDefault = function firstOrDefault<T>(this: Array<T>, predicate?: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): T {
//     return List.prototype.firstOrDefault.bind(this, element)();
// }
// Array.prototype.forEach = function forEach<T>(callbackfn: (value: T, index: number, array: T[]) => void, thisArg?: any): void {
//     return List.prototype.forEach.bind(this, element)();
// }
// Array.prototype.groupBy = function groupBy<T, TResult = T>(grouper: (key: T) => string | number, mapper?: ((element: T) => TResult) | undefined): { [key: string]: TResult[]; } {
//     return List.prototype.groupBy.bind(this, element)();
// }
// Array.prototype.groupJoin = function groupJoin<T, U>(list: List<U>, key1: (k: T) => any, key2: (k: U) => any, result: (first: T, second: List<U>) => any): List<any> {
//     return List.prototype.groupJoin.bind(this, element)();
// }
// Array.prototype.indexOf = function indexOf<T>(this: Array<T>, element: T): number {
//     return List.prototype.indexOf.bind(this, element)();
// }
// Array.prototype.insert = function insert<T>(this: Array<T>, index: number, element: T): void | Error {
//     return List.prototype.insert.bind(this, element)();
// }
Array.prototype.intersect = function intersect<T>(this: Array<T>, source: Array<T>): Array<T> {
    this["_elements"] = this;
    return List.prototype.intersect.bind(this, new List(source))() as unknown as T[];
}
// Array.prototype.linqJoin = function linqJoin<T, U>(list: List<U>, key1: (key: T) => any, key2: (key: U) => any, result: (first: T, second: U) => any): List<any> {
//     return List.prototype.linqJoin.bind(this, element)();
// }
// Array.prototype.last = function last<T>(this: Array<T>, predicate?: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): T {
//     return List.prototype.last.bind(this, element)();
// }
// Array.prototype.lastOrDefault = function lastOrDefault<T>(this: Array<T>, predicate?: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): T {
//     return List.prototype.lastOrDefault.bind(this, element)();
// }
// Array.prototype.max = function max<T>(this: Array<T>, selector?: (value: T, index: number, array: T[]) => number): number {
//     return List.prototype.max.bind(this, element)();
// }
// Array.prototype.min = function min<T>(this: Array<T>, selector?: (value: T, index: number, array: T[]) => number): number {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.ofType = function ofType<T, U>($type: any): List<U> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.orderBy = function orderBy<T>(this: Array<T>, keySelector: (key: T) => any, comparer?: ((a: T, b: T) => number) | undefined): List<T> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.orderByDescending = function orderByDescending<T>(this: Array<T>, keySelector: (key: T) => any, comparer?: ((a: T, b: T) => number) | undefined): List<T> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.thenBy = function thenBy<T>(this: Array<T>, keySelector: (key: T) => any): List<T> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.thenByDescending = function thenByDescending<T>(this: Array<T>, keySelector: (key: T) => any): List<T> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.remove = function remove<T>(this: Array<T>, element: T): boolean {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.removeAll = function removeAll<T>(this: Array<T>, predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): List<T> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.removeAt = function removeAt<T>(this: Array<T>, index: number): void {
//     return List.prototype.add.bind(this, element)();
// }

// Array.prototype.select = function select<T, TOut>(selector: (element: T, index: number) => TOut): List<TOut> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.selectMany = function selectMany<T, TOut extends List<any>>(selector: (element: T, index: number) => TOut): TOut {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.sequenceEqual = function sequenceEqual<T>(this: Array<T>, list: List<T>): boolean {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.single = function single<T>(predicate?: ((value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean) | undefined): T {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.single = function singleOrDefault<T>(predicate?: ((value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean) | undefined): T {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.skip = function skip<T>(amount: number): List<T> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.skipWhile = function skipWhile<T>(predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): List<T> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.sum = function sum<T>(this: Array<T>, transform?: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => number): number {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.take = function take<T>(this: Array<T>, amount: number): List<T> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.takeWhile = function takeWhile<T>(this: Array<T>, predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): List<T> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.toArray = function toArray<T>(this: Array<T>, ): T[] {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.toDictionary = function toDictionary<T, TKey, TValue>(this: Array<T>, key: TKey, value?: TValue) {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.toList = function toList<T>(this: Array<T>, ): List<T> {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.toLookup = function toLookup<T, TResult>(keySelector: (key: T) => string | number, elementSelector: (element: T) => TResult): { [key: string]: TResult[]; } {
//     return List.prototype.add.bind(this, element)();
// }
// Array.prototype.union = function union<T>(this: Array<T>, list: List<T>): List<T> {
//     return List.prototype.add.bind(this, element)();
// }
Array.prototype.where = function where<T>(this: Array<T>, predicate: (value?: T | undefined, index?: number | undefined, list?: T[] | undefined) => boolean): Array<T> {
    this["_elements"] = this;
    return List.prototype.where.bind(this, predicate)().toArray();
}
// Array.prototype.zip = function zip<T, U, TOut>(list: List<U>, result: (first: T, second: U) => TOut): List<TOut> {
//     return List.prototype.add.bind(this, element)();
// }
