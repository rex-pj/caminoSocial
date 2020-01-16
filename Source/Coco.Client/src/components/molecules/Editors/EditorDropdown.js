import React, { useState, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styled from "styled-components";

const Root = styled.div`
  position: relative;
  display: inline-block;
  min-width: 125px;
  z-index: 2;
`;

const Dropdown = styled.div`
  cursor: pointer;
  font-size: 0.8rem;
  padding: ${p => p.theme.size.exTiny} ${p => p.theme.size.tiny};
  height: auto;
  min-width: inherit;
  border: 1px solid ${p => p.theme.color.neutral};
  border-radius: ${p => p.theme.borderRadius.normal};
  position: relative;

  > span {
    color: inherit;
  }
`;

const SelectDropdown = styled.ul`
  cursor: pointer;
  font-size: 0.8rem;
  height: auto;
  position: absolute;
  left: 0;
  top: calc(100% + 1px);
  min-width: 110%;
  border-radius: ${p => p.theme.borderRadius.normal};
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  list-style: none;
  padding-left: 0;
  overflow: hidden;
`;

const Option = styled.li`
  background: ${p => p.theme.color.lighter};
  padding: ${p => p.theme.size.exTiny} ${p => p.theme.size.tiny};
  border-bottom: 1px solid ${p => p.theme.color.light};
  color: ${p => p.theme.color.primaryLight};
  font-weight: 600;

  :hover {
    background-color: ${p => p.theme.color.light};
  }
`;

const ButtonCaret = styled.span`
  position: absolute;
  right: ${p => p.theme.size.exTiny};
  top: 0;
  bottom: 0;
  margin: auto;
  margin-top: 4px;
`;

export default props => {
  const { className, options, placeholder } = props;
  const [isShown, setShown] = useState(false);
  const currentRef = React.createRef();
  const onToggle = (e, value) => {
    if (props.actived !== value) {
      const event = {
        ...e,
        target: {
          ...e.target,
          value
        }
      };
      props.onToggle(event);
      setShown(false);
    }
  };

  const toggleDropdown = e => {
    e.stopPropagation();
    setShown(() => {
      return !isShown;
    });
  };

  useEffect(() => {
    document.addEventListener("click", onHide, false);
    return () => {
      document.removeEventListener("click", onHide);
    };
  });

  const onHide = e => {
    if (!currentRef.current.contains(e.target)) {
      setShown(false);
    }
  };

  const stopPropagation = e => {
    e.stopPropagation();
  };

  const current = options
    ? options.find(element => {
        return element.style === props.actived;
      })
    : {};

  return (
    <Root>
      <Dropdown className={className} onClick={toggleDropdown} ref={currentRef}>
        <span>{current ? current.label : placeholder}</span>
        <ButtonCaret>
          <FontAwesomeIcon icon="caret-down" />
        </ButtonCaret>
      </Dropdown>
      <SelectDropdown onClick={stopPropagation}>
        {isShown
          ? options.map(heading => (
              <Option
                key={heading.style}
                onClick={e => onToggle(e, heading.style)}
              >
                {heading.label}
              </Option>
            ))
          : null}
      </SelectDropdown>
    </Root>
  );
};
