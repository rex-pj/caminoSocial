import React from "react";
import styled from "styled-components";
import { ActionButton } from "../../molecules/ButtonGroups";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ImageRound } from "../../atoms/Images";
import { secondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { ButtonIconPrimary } from "../../molecules/ButtonIcons";
import Overlay from "../../atoms/Overlay";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

const ContentTopbar = styled.div`
  margin-bottom: 0;
`;

const Title = styled(secondaryTitleLink)`
  margin-bottom: 0;
`;

const PostActions = styled.div`
  text-align: right;
`;

const Description = styled.p`
  margin-bottom: 0;
`;

const InteractiveToolbar = styled.div`
  border-top: 1px solid ${(p) => p.theme.color.secondaryDivide};
  padding: ${(p) => p.theme.size.exSmall} ${(p) => p.theme.size.distance};
`;

const InteractiveItem = styled.li`
  margin-right: ${(p) => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const Cover = styled.div`
  position: relative;
  max-height: 250px;
  overflow: hidden;
`;

const CoverImage = styled(ImageRound)`
  border-bottom-left-radius: 0;
  border-bottom-right-radius: 0;
`;

const FollowButton = styled(ButtonIconPrimary)`
  padding: ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.fontSize.small};
  line-height: 1;

  position: absolute;
  bottom: ${(p) => p.theme.size.distance};
  right: ${(p) => p.theme.size.distance};
  z-index: 1;
`;

const ThumbnailOverlay = styled(Overlay)`
  height: 80px;
  top: auto;
  bottom: 0;
`;

export default (props) => {
  const { community } = props;

  return (
    <Panel>
      <Cover>
        <AnchorLink to={community.url}>
          <CoverImage src={community.thumbnailUrl} alt="" />
          <ThumbnailOverlay />
        </AnchorLink>

        <FollowButton icon="handshake" size="sm">
          Join
        </FollowButton>
      </Cover>

      <PanelHeader>
        <ContentTopbar>
          <div className="row g-0">
            <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
              <Title>
                <AnchorLink to={community.url}>{community.name}</AnchorLink>
              </Title>
            </div>

            <div className="col col-4 col-sm-3 col-md-2 col-lg-1">
              <PostActions>
                <ActionButton>
                  <FontAwesomeIcon icon="angle-down" />
                </ActionButton>
              </PostActions>
            </div>
          </div>
        </ContentTopbar>
      </PanelHeader>
      <PanelBody>
        <div className="panel-content">
          <Description>{community.description}</Description>
        </div>
      </PanelBody>
      <InteractiveToolbar>
        <HorizontalList>
          <InteractiveItem>
            <FontButtonItem
              icon="users"
              title="Followers"
              dynamicText={community.followingNumber}
            />
          </InteractiveItem>
        </HorizontalList>
      </InteractiveToolbar>
    </Panel>
  );
};
