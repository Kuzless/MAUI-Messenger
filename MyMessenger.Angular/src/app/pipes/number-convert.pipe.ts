import { DecimalPipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'numberConvert',
  standalone: true,
})
export class NumberConvertPipe implements PipeTransform {
  constructor(private decimalPipe: DecimalPipe) {}

  transform(value: string = '', country = 'en-US'): string {
    if (value == null) {
      return '';
    }

    let transformedValue = this.decimalPipe.transform(value, '1.2-2');

    switch (country) {
      case 'de-DE':
        return transformedValue?.replace('.', ',') ?? '';

      case 'fr-FR':
        return transformedValue?.replace('.', ' ') ?? '';

      case 'ja-JP':
        return transformedValue ?? '';

      default:
        return transformedValue ?? '';
    }
  }
}
