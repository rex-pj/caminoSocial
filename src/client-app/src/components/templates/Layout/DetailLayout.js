import React from "react";
import styled from "styled-components";
import { PageColumnPanel } from "../../atoms/Panels";
import AdsList from "../../organisms/Ads/AdsList";
import AuthorCard from "../../organisms/ProfileCard/AuthorCard";
import {
  FarmSuggestions,
  CommunitySuggestions,
  ConnectionSuggestions,
} from "../../organisms/Suggestions";

const Wrapper = styled.div`
  margin-top: 30px;
  > .row {
    margin-left: -12px;
    margin-right: -12px;
  }

  > .row > .col {
    padding: 0 12px;
  }
`;

// The layout or article or farm detail page
export default (props) => {
  const { author, children } = props;
  return (
    <Wrapper className="container-fluid px-lg-5">
      <div className="row px-lg-3">
        <div className="col col-4 col-sm-4 col-md-2 col-lg-2">
          <PageColumnPanel>
            <AuthorCard author={author} />
          </PageColumnPanel>
        </div>
        <div className="col col-8 col-sm-8 col-md-7 col-lg-7">{children}</div>
        <div className="col col-12 col-sm-12 col-md-3 col-lg-3">
          <PageColumnPanel>
            <FarmSuggestions />
          </PageColumnPanel>
          <PageColumnPanel>
            <AdsList />
          </PageColumnPanel>
          <PageColumnPanel>
            <CommunitySuggestions />
          </PageColumnPanel>
          <PageColumnPanel>
            <AdsList />
          </PageColumnPanel>
          <PageColumnPanel>
            <ConnectionSuggestions />
          </PageColumnPanel>
          <PageColumnPanel>
            <AdsList />
          </PageColumnPanel>
        </div>
      </div>
    </Wrapper>
  );
};