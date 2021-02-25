import React, {
  Fragment,
  useState,
  useRef,
  useEffect,
  useContext,
} from "react";
import styled from "styled-components";
import { withRouter } from "react-router-dom";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import ListItemCollapseCover from "../../molecules/Thumbnails/ListItemCollapseCover";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import AuthorProfile from "../ProfileCard/AuthorProfile";
import { ActionButton } from "../../molecules/ButtonGroups";
import { secondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { AnchorLink } from "../../atoms/Links";
import { convertDateTimeToPeriod } from "../../../utils/DateTimeUtils";
import ContentItemDropdown from "../../molecules/DropdownButton/ContentItemDropdown";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";
import { SessionContext } from "../../../store/context/session-context";
import DeleteConfirmationModal from "../Modals/DeleteConfirmationModal";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 200px;
    border-radius: 0;
  }

  img {
    width: 100%;
  }

  ${ContentItemDropdown} {
    z-index: 10;
  }
`;

const ContentTopbar = styled.div`
  margin-bottom: ${(p) => p.theme.size.exSmall};

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

const PostActions = styled.div`
  text-align: right;
  position: relative;
`;

const PostTitle = styled(secondaryTitleLink)`
  margin-bottom: ${(p) => p.theme.size.distance};
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

const InteractiveToolbar = styled.div`
  border-top: 1px solid ${(p) => p.theme.color.secondaryDivide};
  padding: ${(p) => p.theme.size.exSmall} ${(p) => p.theme.size.distance};
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

export default withRouter((props) => {
  const { article, onOpenDeleteConfirmationModal, location } = props;
  const { createdByIdentityId } = article;
  const [isActionDropdownShown, setActionDropdownShown] = useState(false);
  var { currentUser, isLogin } = useContext(SessionContext);
  const isAuthor =
    currentUser && createdByIdentityId === currentUser.userIdentityId;
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

  useEffect(() => {
    document.addEventListener("click", onActionDropdownHide, false);
    return () => {
      document.removeEventListener("click", onActionDropdownHide);
    };
  });

  const loadCreatedInfo = () => {
    const { creator } = article;
    if (creator) {
      var datePeriod = convertDateTimeToPeriod(article.createdDate);
      creator.info = (
        <Fragment>
          <FontAwesomeIcon icon="calendar-alt" />
          {datePeriod}
        </Fragment>
      );
    }
    return creator;
  };

  const onOpenDeleteConfirmation = () => {
    onOpenDeleteConfirmationModal({
      title: "Delete Farm",
      innerModal: DeleteConfirmationModal,
      message: `Are you sure to delete article "${article.name}"?`,
      id: parseFloat(article.id),
    });
  };

  return (
    <Panel>
      <PanelHeader>
        <ContentTopbar>
          <div className="row g-0">
            <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
              <AuthorProfile profile={loadCreatedInfo()} />
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
        <PostTitle>
          <AnchorLink
            to={{
              pathname: article.url,
              state: { from: location.pathname },
            }}
          >
            {article.name}
          </AnchorLink>
        </PostTitle>
      </PanelHeader>
      <ListItemCollapseCover imageUrl={article.thumbnailUrl} />
      <PanelBody>
        <div className="panel-content">
          <ContentBody>
            <p dangerouslySetInnerHTML={{ __html: article.description }}></p>{" "}
          </ContentBody>
        </div>
      </PanelBody>
      <InteractiveToolbar className="interactive-toolbar">
        <HorizontalList>
          <InteractiveItem>
            <HorizontalReactBar reactionNumber={article.reactionNumber} />
          </InteractiveItem>
          <InteractiveItem>
            <FontButtonItem
              icon="comments"
              title="Discussions"
              dynamicText={article.commentNumber}
            />
          </InteractiveItem>
          <InteractiveItem>
            <FontButtonItem icon="bookmark" title="Save this article" />
          </InteractiveItem>
        </HorizontalList>
      </InteractiveToolbar>
    </Panel>
  );
});
