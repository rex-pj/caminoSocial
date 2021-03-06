import React from "react";
import loadable from "@loadable/component";
import styled from "styled-components";
import { PageColumnPanel } from "../../atoms/Panels";
import CommunityInfo from "../Community/CommunityInfo";
import {
  FarmSuggestions,
  CommunitySuggestions,
  ConnectionSuggestions,
} from "../../organisms/Suggestions";

import Shortcut from "../../organisms/Shortcut";
const Interesting = loadable(() => import("../../organisms/Interesting"));
const AdsList = loadable(() => import("../../organisms/Ads/AdsList"));

const Wrapper = styled.div`
  > .row {
    margin-left: -12px;
    margin-right: -12px;
  }

  > .row > .col {
    padding: 0 12px;
  }
`;

const Column = styled.div`
  margin-top: ${(p) => p.theme.size.normal};
`;

// The layout of Communitys
export default function (props) {
  const { info, children } = props;

  return (
    <Wrapper className="container-fluid px-lg-5">
      <div className="row px-lg-3">
        <div className="col col-4 col-sm-4 col-md-2 col-lg-2">
          <Column>
            <PageColumnPanel>
              <CommunityInfo info={info} />
            </PageColumnPanel>
            <PageColumnPanel>
              <Shortcut />
            </PageColumnPanel>
            <PageColumnPanel>
              <Interesting />
            </PageColumnPanel>
          </Column>
        </div>
        <div className="col col-8 col-sm-8 col-md-7 col-lg-7">{children}</div>
        <div className="col col-12 col-sm-12 col-md-3 col-lg-3">
          <Column>
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
          </Column>
        </div>
      </div>
    </Wrapper>
  );
}
