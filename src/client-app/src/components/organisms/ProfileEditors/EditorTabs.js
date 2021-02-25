import React, { Fragment } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import styled from "styled-components";

const EditorTabs = styled.div`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  .tabs-bar button {
    background-color: transparent;
    font-weight: normal;
    border-color: transparent;
    color: ${(p) => p.theme.color.neutralText};
  }
  .tabs-bar button.actived {
    background-color: ${(p) => p.theme.color.primaryBg};
    color: ${(p) => p.theme.color.lightText};
  }
`;

export default function (props) {
  const { editorMode, onToggleCreateMode } = props;

  return (
    <Fragment>
      <EditorTabs>
        <div className="tabs-bar">
          <ButtonPrimary
            size="xs"
            className={`me-1${editorMode === "ARTICLE" ? " actived" : ""}`}
            onClick={() => onToggleCreateMode("ARTICLE")}
          >
            <span>
              <FontAwesomeIcon
                icon="newspaper"
                className="me-1"
              ></FontAwesomeIcon>
              Create Post
            </span>
          </ButtonPrimary>
          <ButtonPrimary
            size="xs"
            onClick={() => onToggleCreateMode("FARM")}
            className={`me-1${editorMode === "FARM" ? " actived" : ""}`}
          >
            <span>
              <FontAwesomeIcon
                icon="warehouse"
                className="me-1"
              ></FontAwesomeIcon>
              Create Farm
            </span>
          </ButtonPrimary>
          <ButtonPrimary
            size="xs"
            onClick={() => onToggleCreateMode("PRODUCT")}
            className={`me-1${editorMode === "PRODUCT" ? " actived" : ""}`}
          >
            <span>
              <FontAwesomeIcon
                icon="apple-alt"
                className="me-1"
              ></FontAwesomeIcon>
              Create Product
            </span>
          </ButtonPrimary>
        </div>
      </EditorTabs>
    </Fragment>
  );
}
