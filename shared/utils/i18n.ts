/**
 * @file 多语言工具
 * @author 五灵
 */

import { IntlFormat } from "kiwi-intl";
import * as enUsLangs from "../../.kiwi/en-US";
import * as zhCNLangs from "../../.kiwi/zh-CN";
import * as zhTWLangs from "../../.kiwi/zh-TW";

export enum LangEnum {
    "zh-CN" = "zh-CN",
    "en-US" = "en-US",
    "zh-TW" = "zh-TW"
}


const langs = {
    "en-US": enUsLangs.default,
    "zh-CN": zhCNLangs.default,
    "zh-TW": zhTWLangs.default
};
// 从 Cookie 中取语言值, 默认为 zh-CN

let curLang = "zh-CN";

export const I18N = IntlFormat.init(curLang, langs);
