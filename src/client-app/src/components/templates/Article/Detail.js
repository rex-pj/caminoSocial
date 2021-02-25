import React, { Fragment, useRef, useState, useEffect } from "react";
import { withRouter } from "react-router-dom";
import { PanelDefault } from "../../atoms/Panels";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import { PrimaryTitle } from "../../atoms/Titles";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { PanelBody, PanelHeading } from "../../atoms/Panels";
import { convertDateTimeToPeriod } from "../../../utils/DateTimeUtils";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Dropdown from "../../molecules/DropdownButton/Dropdown";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";
import { ActionButton } from "../../molecules/ButtonGroups";
import DeleteConfirmationModal from "../../organisms/Modals/DeleteConfirmationModal";

const Title = styled(PrimaryTitle)`
  color: ${(p) => p.theme.color.primaryText};
`;

const PostActions = styled.div`
  .dropdown-action {
    float: right;
  }
  position: relative;
`;

const ContentTopBar = styled.div`
  font-size: ${(p) => p.theme.fontSize.tiny};
  color: ${(p) => p.theme.color.neutralText};

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

const ContentBody = styled.div`
  padding: 0 0 ${(p) => p.theme.size.distance} 0;
`;

const InteractiveItem = styled.li`
  margin-right: ${(p) => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const PostThumbnail = styled.div`
  margin-top: ${(p) => p.theme.size.exTiny};
`;

const DropdownList = styled(Dropdown)`
  position: absolute;
  right: 0;
  top: ${(p) => p.theme.size.medium};
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

export default withRouter(function (props) {
  const { article, onOpenDeleteConfirmationModal } = props;
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
      pathname: `/articles/update/${article.id}`,
      state: {
        from: props.location.pathname,
      },
    });
  };

  const onOpenDeleteConfirmation = () => {
    onOpenDeleteConfirmationModal({
      title: "Delete Farm",
      innerModal: DeleteConfirmationModal,
      message: `Are you sure to delete article "${article.name}"?`,
      id: parseFloat(article.id),
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
        <PanelHeading>
          <PostActions ref={currentRef}>
            <div className="row">
              <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
                <Title>{article.name}</Title>
                <ContentTopBar>
                  <FontAwesomeIcon icon="calendar-alt" />
                  <span>{convertDateTimeToPeriod(article.createdDate)}</span>
                </ContentTopBar>
              </div>
              <div className="col col-4 col-sm-3 col-md-2 col-lg-1">
                <ActionButton
                  className="dropdown-action"
                  onClick={onActionDropdownShow}
                >
                  <FontAwesomeIcon icon="angle-down" />
                </ActionButton>
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
          </PostActions>
        </PanelHeading>
        {article.thumbnailUrl ? (
          <PostThumbnail>
            <Thumbnail src={article.thumbnailUrl} alt="" />
          </PostThumbnail>
        ) : null}

        <PanelBody>
          <div className="clearfix">
            <div>
              <ContentBody>
                <span
                  dangerouslySetInnerHTML={{ __html: article.content }}
                ></span>
              </ContentBody>

              <div className="interactive-toolbar">
                <HorizontalList>
                  <InteractiveItem>
                    <HorizontalReactBar
                      reactionNumber={article.reactionNumber}
                    />
                  </InteractiveItem>
                  <InteractiveItem>
                    <FontButtonItem
                      icon="comments"
                      title="Discussions"
                      dynamicText={article.commentNumber}
                    />
                  </InteractiveItem>
                  <InteractiveItem>
                    <FontButtonItem icon="bookmark" title="Đánh dấu" />
                  </InteractiveItem>
                </HorizontalList>
              </div>
            </div>
          </div>
        </PanelBody>
      </PanelDefault>
    </Fragment>
  );
});
