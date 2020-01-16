import * as moment from 'moment';

export class DateHelpers {
    static getLocaleDateFormat(locale: string): string {
        return moment.localeData(locale).longDateFormat('L');
    }

    static getLocaleTimeFormat(locale: string): string {
        return moment.localeData(locale).longDateFormat('LT');
    }

    static getLocaleShortDateFromUtc(date: Date, locale: string): string {
        if (date) {
            return moment.utc(date).local().format(this.getLocaleDateFormat(locale));
        }
        return '';
    }

    static getLocaleShortDate(date: Date, locale: string, includeTime?: boolean): string {
        let format: string = this.getLocaleDateFormat(locale);
        if (includeTime) {
            format = `${format} ${this.getLocaleTimeFormat(locale)}`;
        }
        return moment(date).format(format);
    }

    static getLocaleDateFromUtcDate(date: Date, locale: string): Date {
        moment.locale(locale);
        return moment.utc(date).local().toDate();
    }

    static localizeDate(utcDate: Date, locale: string): { localeDate: Date, textDate: string } {
        let localeDate: Date = this.getLocaleDateFromUtcDate(utcDate, locale);
        let textDate: string = this.getLocaleShortDate(localeDate, locale);
        return { localeDate, textDate };
    }

    static isFuture(date: Date): boolean {
        return new Date(date) > new Date();
    }

    static isPast(date: Date): boolean {
        return new Date(date) < new Date();
    }
}
