import { Directive, ElementRef, Input } from '@angular/core';
import { HostListener } from '@angular/core';

@Directive({
  selector: '[appZoom]',
  standalone: true,
})
export class ZoomDirective {
  @Input() appZoom = 0;

  constructor(private el: ElementRef) {}

  @HostListener('mouseenter') onMouseEnter() {
    this.zoom(this.calculateZoom(this.appZoom, true));
  }
  @HostListener('mouseleave') onMouseLeave() {
    this.zoom(this.calculateZoom(this.appZoom, false));
  }

  private zoom(fontSize: string) {
    this.el.nativeElement.style.fontSize = fontSize;
  }
  private calculateZoom(fontSize: number, increase: boolean): string {
    if (increase) {
      return `${fontSize * 1.2}px`;
    }
    return `${fontSize}px`;
  }
}
