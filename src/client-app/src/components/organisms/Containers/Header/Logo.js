import React from "react";
import styled from "styled-components";
import { AnchorLink } from "../../../atoms/Links";
import { PageInfo } from "../../../../utils/Constants";
import { Image } from "../../../atoms/Images";

const LogoImageLink = styled.a`
  float: left;
  height: calc(${(p) => p.theme.size.normal} - 2px);
  margin-right: 5px;
  margin: 2px 5px 2px 0;

  ${Image} {
    height: 100%;
  }
`;

const TitleLink = styled(AnchorLink)`
  float: left;
  height: ${(p) => p.theme.size.normal};
  :hover {
    text-decoration: none;
  }
`;

const LogoTitle = styled.h2`
  display: inline-block;
  color: ${(p) => p.theme.color.neutralText};
  font-family: "Poppins", sans-serif;
  font-size: 28px;
  margin: 1px 0;
  height: ${(p) => p.theme.size.normal};
  background: ${(p) => p.theme.gradientColor.secondary};
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  font-weight: 600;
`;

const TitleHead = styled.span`
  margin-right: 3px;
  color: inherit;
  height: ${(p) => p.theme.size.normal};
`;

const TitleTail = styled.span`
  color: inherit;
  height: ${(p) => p.theme.size.normal};
`;

export default () => {
  return (
    <div className="clearfix">
      <LogoImageLink href="/">
        <Image
          src={`${process.env.PUBLIC_URL}/images/logo.png`}
          alt={`${PageInfo.BrandName} ${PageInfo.BrandDescription}`}
        />
      </LogoImageLink>
      <TitleLink to="/">
        <LogoTitle>
          <TitleHead>{PageInfo.BrandName}</TitleHead>
          <TitleTail>{PageInfo.BrandDescription}</TitleTail>
        </LogoTitle>
      </TitleLink>
    </div>
  );
};
