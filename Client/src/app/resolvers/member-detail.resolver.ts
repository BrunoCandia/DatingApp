import { ResolveFn } from '@angular/router';
import { Member } from '../models/member';
import { inject } from '@angular/core';
import { MemberService } from '../services/member.service';

export const memberDetailResolver: ResolveFn<Member | null> = (route, state) => {
  const memberService = inject(MemberService);

  const userName = route.paramMap.get('userName');

  if (!userName) return null

  return memberService.getMember(userName);
};
