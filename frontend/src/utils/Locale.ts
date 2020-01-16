import { DateHelpers } from './DateHelpers';

const localStorageKeyNameForLocale: string = 'lang';
const defaultLocaleFromResources: string = 'en';
const defaultLocale: string = 'en-GB';

export class Locale {
    static get(): string | null {
        return localStorage.getItem(localStorageKeyNameForLocale);
    }
    static set(locale: string): void {
        locale = locale === defaultLocaleFromResources ? defaultLocale : locale;
        localStorage.setItem(localStorageKeyNameForLocale, locale);
    }
    static localizeDate(utcDate: Date): { localeDate: Date, textDate: string } {
      let aux = this.get();  
      return DateHelpers.localizeDate(utcDate, aux ? aux : '');
    }
}
