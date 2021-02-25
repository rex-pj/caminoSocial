import React, { Fragment, useRef, useEffect, useState } from "react";
import styled from "styled-components";
import { withRouter } from "react-router-dom";
import { PrimaryTitle } from "../../atoms/Titles";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { PanelBody, PanelDefault } from "../../atoms/Panels";
import { ActionButton } from "../../molecules/ButtonGroups";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Breadcrumb from "../../organisms/Navigation/Breadcrumb";
import { ButtonIconPrimary } from "../../molecules/ButtonIcons";
import ThumbnailSlider from "../../organisms/ThumbnailSlider";
import Dropdown from "../../molecules/DropdownButton/Dropdown";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";
import DeleteConfirmationModal from "../../organisms/Modals/DeleteConfirmationModal";

const Title = styled(PrimaryTitle)`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  color: ${(p) => p.theme.color.primaryText};
`;

const ContentBody = styled.div`
  padding: 0 0 ${(p) => p.theme.size.distance} 0;
`;

const InteractiveItem = styled.li`
  margin-right: ${(p) => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const TopBarInfo = styled.div`
  color: ${(p) => p.theme.color.neutralText};
  font-size: ${(p) => p.theme.fontSize.tiny};
  margin-bottom: ${(p) => p.theme.size.distance};
  position: relative;

  ${ModuleMenuListItem} {
    margin-top: 0;
    margin-bottom: 0;
    border-bottom: 1px solid ${(p) => p.theme.color.secondaryDivide};
  }

  ${ModuleMenuListItem}:last-child {
    border-bottom: 0;
  }

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

const PostActions = styled.div`
  text-align: right;
  color: ${(p) => p.theme.color.neutralText};
  position: relative;

  button {
    vertical-align: middle;
  }
`;

const BreadCrumbNav = styled(Breadcrumb)`
  border: 0;
  border-bottom: 1px solid ${(p) => p.theme.color.secondaryDivide};
  border-radius: 0;
  margin-bottom: 0;
  li {
    padding-top: ${(p) => p.theme.size.tiny};
    padding-left: ${(p) => p.theme.size.tiny};
  }
`;

const FollowButton = styled(ButtonIconPrimary)`
  padding: ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.fontSize.small};
  line-height: 1;

  position: absolute;
  top: 0;
  right: 15px;
  z-index: 1;
`;

const DropdownList = styled(Dropdown)`
  position: absolute;
  right: 0;
  top: ${(p) => p.theme.size.normal};
  background: ${(p) => p.theme.color.whiteBg};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  min-width: calc(${(p) => p.theme.size.large} * 3);
  border-radius: ${(p) => p.theme.borderRadius.normal};
  padding: ${(p) => p.theme.size.exTiny} 0;

  ${ModuleMenuListItem} {
    border-bottom: 1px solid ${(p) => p.theme.color.secondaryDivide};
  }

  ${ModuleMenuListItem}:last-child {
    border-bottom: 0;
  }

  ${ModuleMenuListItem} span {
    display: block;
    margin-bottom: 0;

    padding: ${(p) => p.theme.size.tiny} ${(p) => p.theme.size.tiny};
    cursor: pointer;
    text-align: left;

    :hover {
      background-color: ${(p) => p.theme.color.lightBg};
    }
  }
`;

export default withRouter((props) => {
  const { farm, breadcrumbs, onOpenDeleteConfirmationModal } = props;
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

  const onOpenDeleteConfirmation = () => {
    onOpenDeleteConfirmationModal({
      title: "Delete Farm",
      innerModal: DeleteConfirmationModal,
      message: `Are you sure to delete farm "${farm.name}"?`,
      id: parseFloat(farm.id),
    });
  };

  useEffect(() => {
    document.addEventListener("click", onActionDropdownHide, false);
    return () => {
      document.removeEventListener("click", onActionDropdownHide);
    };
  });

  return (
    <Fragment>
      <PanelDefault>
        {farm.images ? (
          <ThumbnailSlider
            currentImage={farm.images[0]}
            images={farm.images}
            numberOfDisplay={5}
          />
        ) : null}

        <BreadCrumbNav list={breadcrumbs} />
        <PanelBody>
          <TopBarInfo>
            <div className="row">
              <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
                <Title>{farm.name}</Title>
                <FontAwesomeIcon icon="map-marker-alt" />
                <span>{farm.address}</span>
              </div>
              <div className="col col-4 col-sm-3 col-md-2 col-lg-1">
                <PostActions ref={currentRef}>
                  <ActionButton
                    className="dropdown-action"
                    onClick={onActionDropdownShow}
                  >
                    <FontAwesomeIcon icon="angle-down" />
                  </ActionButton>
                </PostActions>
                {isActionDropdownShown ? (
                  <DropdownList>
                    <ModuleMenuListItem>
                      <span onClick={onEditMode}>
                        <FontAwesomeIcon icon="pencil-alt"></FontAwesomeIcon>{" "}
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
                  </DropdownList>
                ) : null}
              </div>
            </div>
          </TopBarInfo>
          <div className="clearfix">
            <ContentBody>
              <span
                dangerouslySetInnerHTML={{ __html: farm.description }}
              ></span>
            </ContentBody>

            <div className="interactive-toolbar">
              <div className="row">
                <div className="col col-8 col-sm-9 col-md-10 col-lg-10">
                  <HorizontalList>
                    <InteractiveItem>
                      <HorizontalReactBar
                        reactionNumber={farm.reactionNumber}
                      />
                    </InteractiveItem>
                    <InteractiveItem>
                      <FontButtonItem
                        icon="comments"
                        title="Discussions"
                        dynamicText={farm.commentNumber}
                      />
                    </InteractiveItem>
                    <InteractiveItem>
                      <FontButtonItem icon="bookmark" title="Đánh dấu" />
                    </InteractiveItem>
                  </HorizontalList>
                </div>
                <div className="col col-4 col-sm-3 col-md-2 col-lg-2">
                  <FollowButton icon="user-plus" size="sm">
                    Follow
                  </FollowButton>
                </div>
              </div>
            </div>
          </div>
        </PanelBody>
      </PanelDefault>
    </Fragment>
  );
});
