import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PkceComponent } from './pkce.component';

describe('PkceComponent', () => {
  let component: PkceComponent;
  let fixture: ComponentFixture<PkceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PkceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PkceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
