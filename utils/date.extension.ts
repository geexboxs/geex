import _ = require("lodash");
import { addDays, addHours, addMinutes, addSeconds, addMilliseconds, addWeeks, addMonths, addYears } from "date-fns";

declare global {
    // tslint:disable-next-line: interface-name
    interface Date {
        add: (value: { years?: number; months?: number; weeks?: number; days?: number; hours?: number; minutes?: number; seconds?: number; milliseconds?: number; }) => Date;
        format(format: string): string;
        epoch(): number;
    }
    // tslint:disable-next-line: interface-name
    interface String {
        isNotEmpty(): boolean;
    }
}

Date.prototype.epoch = function (this: Date) {
    return Math.round(this.getTime() / 1000);
};

Date.prototype.add = function (value: { years?: number; months?: number; weeks?: number; days?: number, hours?: number, minutes?: number; seconds?: number; milliseconds?: number }) {
    let result = this;
    if (value.years) {
        result = addYears(result, value.years);
    }
    if (value.months) {
        result = addMonths(result, value.months);
    }
    if (value.weeks) {
        result = addWeeks(result, value.weeks);
    }
    if (value.days) {
        result = addDays(result, value.days);
    }
    if (value.hours) {
        result = addHours(result, value.hours);
    }
    if (value.minutes) {
        result = addMinutes(result, value.minutes);
    }
    if (value.seconds) {
        result = addSeconds(result, value.seconds);
    }
    if (value.milliseconds) {
        result = addMilliseconds(result, value.milliseconds);
    }
    return result;
};

Date.prototype.format = function (this: Date, format: string) {
    const yyyy = this.getFullYear().toString();
    format = format.replace(/yyyy/g, yyyy);
    const MM = (this.getMonth() + 1).toString();
    format = format.replace(/MM/g, (MM[1] ? MM : "0" + MM[0]));
    const dd = this.getDate().toString();
    format = format.replace(/dd/g, (dd[1] ? dd : "0" + dd[0]));
    const HH = this.getHours().toString();
    format = format.replace(/HH/g, (HH[1] ? HH : "0" + HH[0]));
    const mm = this.getMinutes().toString();
    format = format.replace(/mm/g, (mm[1] ? mm : "0" + mm[0]));
    const ss = this.getSeconds().toString();
    format = format.replace(/ss/g, (ss[1] ? ss : "0" + ss[0]));
    return format;
};
