import React, { useEffect } from "react";
import styled from "styled-components";
import { EditorState, Modifier } from "draft-js";
import { DefaultButton } from "./EditorButtons";
import EditorSelection from "./EditorSelection";
import { HEADING_TYPES } from "./Utils";

const Toolbar = styled.div`
  padding: ${(p) => p.theme.size.tiny};
  border-bottom: 1px solid ${(p) => p.theme.color.neutralBg};
  background-color: ${(p) => p.theme.color.primaryBg};
  background-image: ${(p) => p.theme.gradientColor.primaryBg};
  border-top-left-radius: ${(p) => p.theme.borderRadius.normal};
  border-top-right-radius: ${(p) => p.theme.borderRadius.normal};
`;

const EditorButton = styled(DefaultButton)`
  color: ${(p) => p.theme.color.lightText};
  border: 0;
  margin-right: 4px;
  :hover {
    background-color: ${(p) => p.theme.color.secondaryBg};
    color: ${(p) => p.theme.color.whiteText};
  }

  &.actived {
    background-color: ${(p) => p.theme.color.secondaryBg};
    color: ${(p) => p.theme.color.whiteText};
  }
`;

const Divide = styled.span`
  display: inline-block;
  border-right: 1px solid ${(p) => p.theme.color.neutralBg};
  height: 20px;
  width: 0px;
  margin: 0 8px 0 4px;
  vertical-align: middle;
`;

const SelectHeading = styled(EditorSelection)`
  color: ${(p) => p.theme.color.lightText};
  font-weight: 600;
  border: 0;

  :hover {
    background-color: ${(p) => p.theme.color.secondaryBg};
  }
`;

export default (props) => {
  const { editorState, styles } = props;

  const toggleBlockType = (e) => {
    const value = e.target.value;
    props.toggleBlockType(value);
    focusEditor();
  };

  const toggleInlineStyle = (e) => {
    const value = e.target.value;
    props.toggleInlineStyle(value);
  };

  const currentStyle = editorState.getCurrentInlineStyle();
  const selection = editorState.getSelection();
  const blockType = editorState
    .getCurrentContent()
    .getBlockForKey(selection.getStartKey())
    .getType();

  const focusEditor = () => {
    setTimeout(() => {
      props.focusEditor();
    }, 50);
  };

  function removeInlineStyles(currentState) {
    const contentState = currentState.getCurrentContent();
    var inlineStyles = styles.filter((type) => type.type === "inline");
    const contentWithoutStyles = inlineStyles.reduce(
      (state, item) =>
        Modifier.removeInlineStyle(
          state,
          currentState.getSelection(),
          item.style
        ),
      contentState
    );

    return EditorState.push(
      currentState,
      contentWithoutStyles,
      "change-inline-style"
    );
  }

  function removeBlockStyles(currentState) {
    const contentState = currentState.getCurrentContent();
    let newEditorState = currentState;
    let contentWithoutStyles = contentState;
    contentWithoutStyles = Modifier.setBlockType(
      contentWithoutStyles,
      currentState.getSelection(),
      "unstyled"
    );

    return EditorState.push(
      newEditorState,
      contentWithoutStyles,
      "change-block-type"
    );
  }

  const clearFormat = () => {
    const helpers = [];

    helpers.push(removeInlineStyles);

    helpers.push(removeBlockStyles);

    const newEditorState = helpers.reduce(
      (state, helper) => helper(state),
      editorState
    );

    props.clearFormat(newEditorState);
  };

  const onLinkModalOpen = () => {
    if (props.onLinkModalOpen) {
      props.onLinkModalOpen(true);
    }
  };

  const onImageModalOpen = () => {
    if (props.onImageModalOpen) {
      props.onImageModalOpen(true);
    }
  };

  const onRemoveLink = (e) => {
    props.onRemoveLink(e);
  };

  useEffect(() => {
    return () => {
      return clearTimeout();
    };
  });

  const items = styles.map((item, index) => {
    if (item.type === "divide") {
      return <Divide key={item.type + index} />;
    } else if (item.type === "image") {
      return (
        <EditorButton
          icon="image"
          onToggle={onImageModalOpen}
          key={item.type + index}
        />
      );
    } else if (item.type === "link") {
      return (
        <EditorButton
          icon="link"
          onToggle={onLinkModalOpen}
          key={item.type + index}
        />
      );
    } else if (item.type === "unlink") {
      return (
        <EditorButton
          icon="unlink"
          onToggle={onRemoveLink}
          key={item.type + index}
        />
      );
    } else if (item.type === "eraser") {
      return (
        <EditorButton
          icon="eraser"
          onToggle={clearFormat}
          key={item.type + index}
        />
      );
    } else if (item.type === "headingStyle") {
      return (
        <SelectHeading
          key={item.type + index}
          options={HEADING_TYPES}
          actived={blockType}
          onToggle={toggleBlockType}
          placeholder="Heading Styles"
        />
      );
    } else if (item.type && item.type === "inline") {
      return (
        <EditorButton
          key={item.type + index}
          actived={item.style === blockType}
          label={item.label}
          icon={item.icon}
          onToggle={toggleBlockType}
          style={item.style}
        />
      );
    }

    return (
      <EditorButton
        key={item.type + index}
        actived={currentStyle.has(item.style)}
        label={item.label}
        icon={item.icon}
        onToggle={toggleInlineStyle}
        style={item.style}
      />
    );
  });

  return <Toolbar>{items}</Toolbar>;
};
