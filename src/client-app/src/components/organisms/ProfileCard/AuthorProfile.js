import React from "react";
import styled from "styled-components";
import { Image } from "../../atoms/Images";
import { AnchorLink } from "../../atoms/Links";
import NoAvatar from "../../atoms/NoImages/no-avatar";

const CreatorAvatar = styled(AnchorLink)`
  margin-right: ${(p) => p.theme.size.distance};
  display: block;

  ${Image} {
    width: ${(p) => p.theme.size.normal};
    height: ${(p) => p.theme.size.normal};
    border-radius: ${(p) => p.theme.size.normal};
  }
`;

const EmptyAvatar = styled(NoAvatar)`
  width: ${(p) => p.theme.size.normal};
  height: ${(p) => p.theme.size.normal};
  border-radius: ${(p) => p.theme.size.normal};
  font-size: 18px;
`;

const CreatorDetail = styled.div`
  line-height: 1;

  svg {
    margin-right: ${(p) => p.theme.size.exTiny};
    color: inherit;
    vertical-align: middle;
  }

  path {
    color: inherit;
  }
`;

const CreatorName = styled(AnchorLink)`
  font-weight: 700;
  font-size: ${(p) => p.theme.fontSize.small};
  display: inline-block;
  margin-bottom: 5px;
  margin-top: 1px;
  color: ${(p) => p.theme.color.darkText};
`;

const MoreInfo = styled.p`
  font-size: ${(p) => p.theme.fontSize.tiny};
  color: ${(p) => p.theme.color.lightText};
  font-weight: 400;
  margin-bottom: 0;
`;

export default (props) => {
  const { profile, className } = props;
  return (
    <div className={`${className ? className : ""} row g-0`}>
      <div className="col col-auto">
        <CreatorAvatar to={profile.profileUrl}>
          {profile.photoUrl ? (
            <Image src={profile.photoUrl} alt="" />
          ) : (
            <EmptyAvatar />
          )}
        </CreatorAvatar>
      </div>
      <div className="col col-auto">
        <CreatorDetail>
          <CreatorName to={profile.profileUrl}>{profile.name}</CreatorName>
          {profile.info && <MoreInfo>{profile.info}</MoreInfo>}
        </CreatorDetail>
      </div>
    </div>
  );
};
