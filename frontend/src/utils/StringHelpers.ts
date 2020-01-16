export class StringHelpers {
    /**
     * Given a string, replace every substring that is matched by the `match` string
     * with the result of calling `fn` on matched substring. The result will be an
     * array with all odd indexed elements containing the replacements. The primary
     * use case is similar to using String.prototype.replace except for React.
     *
     * React will happily render an array as children of a react element, which
     * makes this approach very useful for tasks like surrounding certain text
     * within a string with react elements.
     *
     * @param {string} str
     * @param {str} match Must contain a matching group
     * @param {function} fn
     * @return {array}
     */
    public static replaceString(str: string, match: string, fn: any) {
        let curCharStart = 0;
        let curCharLen = 0;

        if (str === '') {
            return str;
        } else if (!str) {
            throw new TypeError('First argument to #replaceString must be a string');
        }
        let re = new RegExp(`(${this.escapeRegExp(match)})`, 'gi');
        let result = str.split(re);

        // Apply fn to all odd elements
        for (let i = 1, length = result.length; i < length; i += 2) {
            curCharLen = result[i].length;
            curCharStart += result[i - 1].length;
            result[i] = fn(result[i], i, curCharStart);
            curCharStart += curCharLen;
        }
        return result;
    }

    static escapeRegExp(str : string): string {
        return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, '\\$&');
    }

    static compareStrings(value1: string, value2: string, caseInsensitive?: boolean): number {
        let text1: string = caseInsensitive ? value1.toLowerCase() : value1;
        let text2: string = caseInsensitive ? value2.toLowerCase() : value2;
        if (text1 === text2) {
            return 0;
        }
        else if (text1 < text2) {
            return -1;
        }
        else {
            return 1;
        }
    }

    static compareStringsOrNumber(value1: string, value2: string, caseInsensitive?: boolean): number {
        if (this.isNumeric(value1) && this.isNumeric(value2)) {
            return this.compareNumber(Number(value1), Number(value2));
        }
        else if (this.isNumeric(value1) && !this.isNumeric(value2)) {
            return -1;
        }
        else if (!this.isNumeric(value1) && this.isNumeric(value2)) {
            return 1;
        }
        else {
            return this.compareStrings(value1, value2, caseInsensitive);
        }
    }

    static compareNumber(number1: number, number2: number): number {
        if (number1 === number2) {
            return 0;
        }
        else if (number1 < number2) {
            return -1;
        }
        else {
            return 1;
        }
    }

    static isNumeric(value: any): boolean {
        return !isNaN(Number(value));
    }

    static getNumber(value: string): number {
        let convertedValue: number = Number(value);
        return Number.isNaN(convertedValue) ? parseInt(String(value), 10) : convertedValue;
    }

    // Copied from https://www.meziantou.net/2017/11/06/typescript-nameof-operator-equivalent
    static nameof<T>(key: keyof T, instance?: T): keyof T {
        return key;
    }

    static format(template: string, ...args: string[]): string {
        let returnValue: string = template;
        args.forEach((arg: string, index: number) => returnValue = returnValue.replace(new RegExp(`{[${index}]}`, 'g'), arg));
        return returnValue;
    }
}
