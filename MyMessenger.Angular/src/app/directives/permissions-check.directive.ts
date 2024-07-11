import {
  Directive,
  TemplateRef,
  ViewContainerRef,
  Input,
  OnInit,
} from '@angular/core';
import { UserService } from '../pages/users/services/user.service';

@Directive({
  selector: '[appPermissionsCheck]',
  standalone: true,
})
export class PermissionsCheckDirective implements OnInit {
  @Input() appPermissionsCheck!: string;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.checkPermission();
  }

  private checkPermission() {
    this.userService
      .hasPermission(this.appPermissionsCheck)
      .subscribe((hasPermission) => {
        if (hasPermission) {
          this.viewContainer.createEmbeddedView(this.templateRef);
        } else {
          this.viewContainer.clear();
        }
      });
  }
}
