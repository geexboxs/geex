import KiwiIntl from "kiwi-intl";

export const kiwiIntl = KiwiIntl.init("en-UK", {
    "en-UK": {
        test: "testvalue",
        testTemplate: "you have {value} unread message",
        photo:
            "You have {num, plural, =0 {no photos.} =1 {one photo.} other {# photos.}}"
    },
    "zh-CN": {
        lang: "语言",
    },
});
export const i18n: (key: string, ...args: any[]) => string = kiwiIntl.get;
