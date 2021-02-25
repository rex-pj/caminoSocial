import React, {
  Fragment,
  useRef,
  useState,
  useEffect,
  useContext,
} from "react";
import AuthorProfile from "../ProfileCard/AuthorProfile";
import { HorizontalReactBar } from "../../molecules/Reaction";
import styled from "styled-components";
import { ActionButton } from "../../molecules/ButtonGroups";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ListItemCollapseCover from "../../molecules/Thumbnails/ListItemCollapseCover";
import { secondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { ButtonIconPrimary } from "../../molecules/ButtonIcons";
import { convertDateTimeToPeriod } from "../../../utils/DateTimeUtils";
import ContentItemDropdown from "../../molecules/DropdownButton/ContentItemDropdown";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";
import { SessionContext } from "../../../store/context/session-context";
import { withRouter } from "react-router-dom";
import DeleteConfirmationModal from "../Modals/DeleteConfirmationModal";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 200px;
    border-radius: 0;
  }

  img.thumbnail-img {
    width: 100%;
  }
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

const ContentTopbar = styled.div`
  margin-bottom: 0;

  ${ModuleMenuListItem} {
    margin-top: 0;
    margin-bottom: 0;
    border-bottom: 1px solid ${(p) => p.theme.color.secondaryDivide};
  }

  ${ModuleMenuListItem}:last-child {
    border-bottom: 0;
  }

  ${ModuleMenuListItem} span {
    padding-top: ${(p) => p.theme.size.tiny};
    padding-bottom: ${(p) => p.theme.size.tiny};
  }
`;

const Title = styled(secondaryTitleLink)`
  margin-bottom: 0;
`;

const PostActions = styled.div`
  text-align: right;
  position: relative;
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
  overflow: hidden;

  img {
    border-top-left-radius: ${(p) => p.theme.borderRadius.normal};
    border-top-right-radius: ${(p) => p.theme.borderRadius.normal};
  }
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

const ProfileBox = styled(AuthorProfile)`
  position: absolute;
  bottom: ${(p) => p.theme.size.distance};
  left: ${(p) => p.theme.size.distance};
  z-index: 1;

  a {
    color: ${(p) => p.theme.color.lightText};
  }
`;

const TopBarInfo = styled.div`
  color: ${(p) => p.theme.color.neutralText};
  font-size: ${(p) => p.theme.fontSize.tiny};

  span {
    color: inherit;
    vertical-align: middle;
  }

  svg {
    margin-right: ${(p) => p.theme.size.exTiny};
    color: inherit;
    vertical-align: middle;
  }

  path {
    color: inherit;
  }
`;

export default withRouter((props) => {
  const { farm, onOpenDeleteConfirmationModal, location } = props;
  const { createdByIdentityId } = farm;
  var { currentUser, isLogin } = useContext(SessionContext);
  const isAuthor =
    currentUser && createdByIdentityId === currentUser.userIdentityId;
  const [isActionDropdownShown, setActionDropdownShown] = useState(false);
  const currentRef = useRef();
  const onActionDropdownHide = (e) => {
    if (currentRef.current && !currentRef.current.contains(e.target)) {
      setActionDropdownShown(false);
    }
  };

  const onActionDropdownShow = () => {
    setActionDropdownShown(true);
  };

  const onEditMode = async () => {
    props.history.push({
      pathname: `/farms/update/${farm.id}`,
      state: {
        from: props.location.pathname,
      },
    });
  };

  useEffect(() => {
    document.addEventListener("click", onActionDropdownHide, false);
    return () => {
      document.removeEventListener("click", onActionDropdownHide);
    };
  });

  const loadCreatedInfo = () => {
    const { creator } = farm;
    if (creator) {
      creator.info = "Farmer";
    }
    return creator;
  };

  const onOpenDeleteConfirmation = () => {
    onOpenDeleteConfirmationModal({
      title: "Delete Farm",
      innerModal: DeleteConfirmationModal,
      message: `Are you sure to delete farm "${farm.name}"?`,
      id: parseFloat(farm.id),
    });
  };

  return (
    <Panel>
      <Cover>
        <ListItemCollapseCover imageUrl={farm.thumbnailUrl} />

        <ProfileBox profile={loadCreatedInfo()} />
        <FollowButton icon="user-plus" size="sm">
          Follow
        </FollowButton>
      </Cover>

      <PanelHeader>
        <ContentTopbar>
          <div className="row g-0">
            <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
              <Title>
                <AnchorLink
                  to={{
                    pathname: farm.url,
                    state: { from: location.pathname },
                  }}
                >
                  {farm.name}
                </AnchorLink>
              </Title>

              <TopBarInfo>
                <span className="me-3">
                  <FontAwesomeIcon icon="calendar-alt" />
                  <span>{convertDateTimeToPeriod(farm.createdDate)}</span>
                </span>
                {farm.address ? (
                  <Fragment>
                    <FontAwesomeIcon icon="map-marker-alt" />
                    <span>{farm.address}</span>
                  </Fragment>
                ) : null}
              </TopBarInfo>
            </div>

            <div className="col col-4 col-sm-3 col-md-2 col-lg-1">
              {isLogin ? (
                <PostActions ref={currentRef}>
                  <ActionButton onClick={onActionDropdownShow}>
                    <FontAwesomeIcon icon="angle-down" />
                  </ActionButton>
                </PostActions>
              ) : null}
              {isActionDropdownShown && isAuthor ? (
                <ContentItemDropdown>
                  <ModuleMenuListItem>
                    <span onClick={onEditMode}>
                      <FontAwesomeIcon
                        icon="pencil-alt"
                        className="me-2"
                      ></FontAwesomeIcon>
                      Edit
                    </span>
                  </ModuleMenuListItem>
                  <ModuleMenuListItem>
                    <span onClick={onOpenDeleteConfirmation}>
                      <FontAwesomeIcon
                        icon="trash-alt"
                        className="me-2"
                      ></FontAwesomeIcon>
                      Delete
                    </span>
                  </ModuleMenuListItem>
                </ContentItemDropdown>
              ) : null}
            </div>
          </div>
        </ContentTopbar>
      </PanelHeader>
      <PanelBody>
        <div className="panel-content">
          <Description>
            <span dangerouslySetInnerHTML={{ __html: farm.description }}></span>{" "}
          </Description>
        </div>
      </PanelBody>
      <InteractiveToolbar>
        <HorizontalList>
          <InteractiveItem>
            <HorizontalReactBar reactionNumber={farm.reactionNumber} />
          </InteractiveItem>

          <InteractiveItem>
            <FontButtonItem
              icon="comments"
              title="Discussions"
              dynamicText={farm.commentNumber}
            />
          </InteractiveItem>
        </HorizontalList>
      </InteractiveToolbar>
    </Panel>
  );
});
