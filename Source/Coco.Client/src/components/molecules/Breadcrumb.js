import React, { Fragment } from "react";
import styled from "styled-components";
import { AnchorLink } from "../atoms/Links";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Root = styled.ol`
  padding: 0;
  margin-bottom: 1rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  border: 1px solid ${p => p.theme.color.light};
  border-radius: ${p => p.theme.borderRadius.normal};
`;

const ListItem = styled.li`
  display: inline-block;
  font-size: ${p => p.theme.fontSize.small};
  color: ${p => p.theme.color.secondary};

  border-right: 1px solid ${p => p.theme.color.light};
  border-top-right-radius: ${p => p.theme.borderRadius.large};
  border-bottom-right-radius: ${p => p.theme.borderRadius.large};
  padding: ${p => p.theme.size.tiny};
  min-width: ${p => p.theme.size.normal};
  line-height: 1;
  position: relative;

  &.actived {
    color: ${p => p.theme.color.normal};
    border-right: 0;
  }

  span {
    color: inherit;
  }

  a {
    text-decoration: none;
    color: inherit;
    font-weight: 600;
  }

  svg,
  path {
    color: inherit;
    font-size: ${p => p.theme.fontSize.exSmall};
  }
`;

export default props => {
  const { list, className } = props;
  return (
    <Root className={className}>
      <ListItem>
        <AnchorLink to="/">
          <FontAwesomeIcon icon="home" />
        </AnchorLink>
      </ListItem>
      {list
        ? list.map((item, index) => {
            if (!item.isActived) {
              return (
                <Fragment key={index}>
                  <ListItem title={item.title}>
                    <AnchorLink to={item.url}>{item.title}</AnchorLink>
                  </ListItem>
                </Fragment>
              );
            }

            return (
              <Fragment key={index}>
                <ListItem key={index} className="actived" title={item.title}>
                  <span>{item.title}</span>
                </ListItem>
              </Fragment>
            );
          })
        : null}
    </Root>
  );
};