/**
 * @file 多语言工具
 * @author 五灵
 */

import { IntlFormat } from "kiwi-intl";
import { langs as enUsLangs } from "@workspace/.kiwi/en-US";
import { langs as zhCNLangs } from "@workspace/.kiwi/zh-CN";
import { langs as zhTWLangs } from "@workspace/.kiwi/zh-TW";

export enum LangEnum {
    "zh-CN" = "zh-CN",
    "en-US" = "en-US",
    "zh-TW" = "zh-TW"
}


const langs = {
    "en-US": enUsLangs,
    "zh-CN": zhCNLangs,
    "zh-TW": zhTWLangs
};
// 从 Cookie 中取语言值, 默认为 zh-CN

let curLang = "zh-CN";

export const I18N = IntlFormat.init(curLang, langs);
