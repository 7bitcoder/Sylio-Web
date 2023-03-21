import { Pipe, PipeTransform } from '@angular/core';
import { Truncated } from '../models/truncated';

@Pipe({
  name: 'truncate'
})
export class TruncatePipe implements PipeTransform {

  transform(value: string | null | undefined, limit: number): Truncated {
    value ??= 'null';
    if (value.length < limit) {
      return {
        isTruncated: false,
        originalText: value,
        truncatedText: value,
      }
    }
    return {
      isTruncated: true,
      originalText: value,
      truncatedText: value.slice(0, limit) + '...',
    }
  }
}
